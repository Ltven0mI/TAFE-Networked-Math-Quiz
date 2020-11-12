using CommonClasses.Networking;
using ENet;
using MVVMUtil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using TeacherApp.Services.Interfaces;

namespace TeacherApp.Services
{
    /**********************************************************/
    // Filename:   HostService.cs
    // Purpose:    A service providing a simple to use
    //             - multi-threaded server built on top of ENet.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-11-09
    // Tests:      N/A
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // 
    // [0.1.0] 2020-11-09
    // | [Added]
    // | - Initial class implementation.
    /**********************************************************/

    /// <summary>
    /// A service providing a simple to use multi-threaded server built on top of ENet.
    /// </summary>
    public class HostService : IHostService
    {
        public const int MAX_CONNECTIONS = 1;
        public const byte MAX_CHANNELS = 1;
        private const int MAX_PACKETS_PER_TICK = 10;

        #region Events

        public event IHostService.ClientConnectionEventHandler ClientConnected;
        public event IHostService.ClientConnectionEventHandler ClientDisconnected;
        public event IHostService.ClientConnectionEventHandler ClientTimedout;

        public event IHostService.DataReceivedEventHandler DataReceived;

        public event IHostService.PacketSendFailedEventHandler PacketSendFailed;
        public event IHostService.PacketBroadcastFailedEventHandler PacketBroadcastFailed;

        #endregion Events

        #region Properties

        #region Property - IsRunning
        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                _isRunning = value;
                RaisePropertyChanged(nameof(IsRunning));
            }
        }
        #endregion Property - IsRunning

        #endregion Properties

        private Address _address;

        private bool _isStopRequested;
        private Thread _thread;
        private Dispatcher _dispatcher;

        private ConcurrentQueue<QueuedPacket> _packetQueue;

        public void Start(ushort port)
        {
            // State Validation //
            if (IsRunning)
                throw new InvalidOperationException("Cannot start Host, Host is already running.");

            // Argument Storing //
            _address = new Address() { Port=port };

            // Variable Initialization //
            IsRunning = true;
            _isStopRequested = false;
            _dispatcher = Dispatcher.CurrentDispatcher;
            _packetQueue = new ConcurrentQueue<QueuedPacket>();

            // Thread Creation //
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        public void Shutdown()
        {
            // State Validation //
            if (!IsRunning)
                throw new InvalidOperationException("Cannot shutdown Host, Host is already shutdown.");

            // Stop internal Thread.
            _isStopRequested = true;
            _thread.Join();
        }

        public void SendPacket(uint peerID, byte channelID, byte[] data)
        {
            // State Validation //
            if (!IsRunning)
                throw new InvalidOperationException("Cannot send packet, Host is not currently running.");

            // Enqueue Packet //
            _packetQueue.Enqueue(new QueuedPacket(peerID, channelID, data));
        }

        public void BroadcastPacket(byte channelID, byte[] data)
        {
            // State Validation //
            if (!IsRunning)
                throw new InvalidOperationException("Cannot broadcast packet, Host is not currently running.");

            // Enqueue Packet //
            _packetQueue.Enqueue(new QueuedPacket(default, channelID, data, true));
        }

        private void Run()
        {
            // Create and initialize ENet Host (As a server)
            using Host server = new Host();
            server.Create(_address, MAX_CONNECTIONS, MAX_CHANNELS);

            Peer?[] peers = new Peer?[MAX_CONNECTIONS];

            // Internal loop //
            var isLoopRunning = true;
            while (isLoopRunning)
            {
                // Stop Requested //
                if (_isStopRequested)
                {
                    isLoopRunning = false;

                    // Disconnect Peers //
                    foreach (var peer in peers)
                        peer?.DisconnectNow(default);

                    continue;
                }

                // Process Queued Packets //
                for (int i=0; i<Math.Min(_packetQueue.Count, MAX_PACKETS_PER_TICK); i++)
                {
                    // Dequeue the next QueuedPacket
                    if (!_packetQueue.TryDequeue(out QueuedPacket queuedPacket))
                        break;

                    // Broadcast/Send Queued Packet //
                    string errorMessage = "";
                    if (queuedPacket.IsBroadcast)
                    {
                        if (!TryBroadcastPacket(server, queuedPacket, out errorMessage))
                            _dispatcher.BeginInvoke(() => OnPacketBroadcastFailed(queuedPacket.ChannelID, queuedPacket.Data, errorMessage));
                    }
                    else
                    {
                        if (!TrySendPacket(peers, queuedPacket, out errorMessage))
                            _dispatcher.BeginInvoke(() => OnPacketSendFailed(queuedPacket.PeerID, queuedPacket.ChannelID, queuedPacket.Data, errorMessage));
                    }
                }

                // Service Server //
                if (server.Service(0, out Event netEvent) == 1)
                {
                    switch (netEvent.Type)
                    {
                        // Connect - Triggered when a client successfully connects to the server.
                        case EventType.Connect:
                            peers[netEvent.Peer.ID] = netEvent.Peer;
                            _dispatcher.BeginInvoke(() => OnClientConnected(netEvent.Peer.ID));
                            break;

                        // Disconnect - Triggered when a client disconnects from the server.
                        case EventType.Disconnect:
                            peers[netEvent.Peer.ID] = null;
                            _dispatcher.BeginInvoke(() => OnClientDisconnected(netEvent.Peer.ID));
                            break;

                        // Timeout - Triggered when a connected client timesout.
                        case EventType.Timeout:
                            peers[netEvent.Peer.ID] = null;
                            _dispatcher.BeginInvoke(() => OnClientTimedout(netEvent.Peer.ID));
                            break;

                        // Receive - Triggered when data is received from a connected client.
                        case EventType.Receive:
                            byte[] data = new byte[netEvent.Packet.Length];
                            netEvent.Packet.CopyTo(data);
                            netEvent.Packet.Dispose();
                            _dispatcher.BeginInvoke(() => OnDataReceived(netEvent.Peer.ID, netEvent.ChannelID, data));
                            break;
                    }
                }

                // Sleep the thread to give the CPU a chance to chill lol.
                Thread.Sleep(1);
            }

            // Send any remaining packets
            server.Flush();

            // Cleanup //
            peers = null;
            _dispatcher.BeginInvoke(PostThreadCleanup);
        }

        private void PostThreadCleanup()
        {
            _thread = null;
            _address = default;
            _dispatcher = null;
            _packetQueue.Clear();
            _packetQueue = null;

            IsRunning = false;
        }

        private bool TryBroadcastPacket(Host server, QueuedPacket queuedPacket, out string errorMessage)
        {
            var channelID = queuedPacket.ChannelID;

            errorMessage = null;

            // Ensure ChannelID is valid
            if (channelID < 0 || channelID >= MAX_CHANNELS)
                errorMessage = $"Invalid {nameof(QueuedPacket.ChannelID)}. Was {channelID}, expected between 0 and {MAX_CHANNELS}";

            // If an error occured, skip this packet and report the error
            if (errorMessage != null)
                return false;

            // Send packet to peer.
            var packet = default(Packet);
            packet.Create(queuedPacket.Data);
            server.Broadcast(channelID, ref packet);

            // Return true for success
            return true;
        }

        private bool TrySendPacket(Peer?[] peers, QueuedPacket queuedPacket, out string errorMessage)
        {
            var peerID = queuedPacket.PeerID;
            var channelID = queuedPacket.ChannelID;

            errorMessage = null;
            Peer? peer = null;

            // Ensure PeerID is not out of range.
            if (peerID < 0 || peerID >= MAX_CONNECTIONS)
                errorMessage = $"Invalid {nameof(QueuedPacket.PeerID)}. Was {peerID}, expected between 0 and {MAX_CONNECTIONS}";
            // Ensure the Peer exists.
            else if ((peer = peers[peerID]) == null)
                errorMessage = $"No peer is connected with the ID: '{peerID}'";
            // Ensure ChannelID is valid
            else if (channelID < 0 || channelID >= MAX_CHANNELS)
                errorMessage = $"Invalid {nameof(QueuedPacket.ChannelID)}. Was {channelID}, expected between 0 and {MAX_CHANNELS}";

            // If an error occured, skip this packet and report the error
            if (errorMessage != null)
                return false;

            // Send packet to peer.
            var packet = default(Packet);
            packet.Create(queuedPacket.Data);
            peer.Value.Send(channelID, ref packet);

            // Return true for success
            return true;
        }

        #region Callbacks

        private void OnClientConnected(uint peerID)
        {
            ClientConnected?.Invoke(this, new IHostService.ClientConnectionEventArgs()
                { PeerID=peerID });
        }

        private void OnClientDisconnected(uint peerID)
        {
            ClientDisconnected?.Invoke(this, new IHostService.ClientConnectionEventArgs()
                { PeerID = peerID });
        }

        private void OnClientTimedout(uint peerID)
        {
            ClientTimedout?.Invoke(this, new IHostService.ClientConnectionEventArgs()
                { PeerID = peerID });
        }

        private void OnDataReceived(uint peerID, byte channelID, byte[] data)
        {
            DataReceived?.Invoke(this, new IHostService.DataReceivedEventArgs()
                { PeerID=peerID, ChannelID=channelID, Data=data });
        }

        private void OnPacketSendFailed(uint peerID, byte channelID, byte[] data, string errorMessage)
        {
            PacketSendFailed?.Invoke(this, new IHostService.PacketSendFailedEventArgs()
                { PeerID=peerID, ChannelID=channelID, Data=data, ErrorMessage=errorMessage });
        }

        private void OnPacketBroadcastFailed(byte channelID, byte[] data, string errorMessage)
        {
            PacketBroadcastFailed?.Invoke(this, new IHostService.PacketBroadcastFailedEventArgs()
                { ChannelID = channelID, Data = data, ErrorMessage = errorMessage });
        }

        #endregion Callbacks

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;


        /**********************************************************/
        // Method:  protected void RaisePropertyChanged(string propertyName).
        // Purpose: Invokes PropertyChanged for the property
        //          - identified by 'propertyName'.
        // Inputs:  string propertyName
        // Throws:  UnknownPropertyException - when no property
        //          - exists with the name 'propertyName'.
        /**********************************************************/

        /// <summary>
        /// Invokes <see cref="PropertyChanged"/> for the property
        /// identified by <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <exception cref="UnknownPropertyException">
        /// Thrown when no property exists with the name <paramref name="propertyName"/>.
        /// </exception>
        protected void RaisePropertyChanged(string propertyName)
        {
            // if the desired property exists: invoke PropertyChanged
            VerifyPropertyName(propertyName);
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }


        /**********************************************************/
        // Method:  public void VerifyPropertyName(string propertyName)
        // Purpose: Verifies that a property exists with the name
        //          - 'propertyName'.
        // Inputs:  string propertyName
        // Throws:  UnknownPropertyException - when no property
        //          - exists with the name 'propertyName'.
        /**********************************************************/

        /// <summary>
        /// Verifies that a property exists with the name <paramref name="propertyName"/>.
        /// </summary>
        /// <param name="propertyName">The name of the property to verify.</param>
        /// <exception cref="UnknownPropertyException">
        /// Thrown when no property exists with the name <paramref name="propertyName"/>.
        /// </exception>
        protected void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] != null)
                return;
            string msg = $"Invalid property name: {propertyName}.";
            throw new UnknownPropertyException(propertyName, msg);
        }

        #endregion INotifyPropertyChanged Members

        private struct QueuedPacket
        {
            public uint PeerID { get; }
            public byte ChannelID { get; }
            public byte[] Data { get; }
            public bool IsBroadcast { get; }

            public QueuedPacket(uint peerID, byte channelID, byte[] data, bool isBroadcast=false)
            {
                PeerID = peerID;
                ChannelID = channelID;
                Data = new byte[data.Length];
                Array.Copy(data, Data, data.Length);
                IsBroadcast = isBroadcast;
            }
        }
    }
}
