using ENet;
using MVVMUtil;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using StudentApp.Services.Interfaces;

namespace StudentApp.Services
{
    public class PeerService : IPeerService
    {
        public const byte MAX_CHANNELS = 1;
        private const int MAX_PACKETS_PER_TICK = 10;

        #region Events

        public event IPeerService.ConnectionEventHandler ConnectionSuccessful;
        public event IPeerService.ConnectionEventHandler ConnectionUnsuccessful;
        public event IPeerService.ConnectionEventHandler ConnectionLost;
        public event IPeerService.ConnectionEventHandler ConnectionCanceled;
        public event IPeerService.ConnectionEventHandler ConnectionClosed;

        public event IPeerService.DataReceivedEventHandler DataReceived;

        public event IPeerService.PacketSendFailedEventHandler PacketSendFailed;

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

        #region Property - ConnectionState
        private IPeerService.ConnectionStates _connectionState;
        public IPeerService.ConnectionStates ConnectionState
        {
            get => _connectionState;
            private set
            {
                _connectionState = value;
                RaisePropertyChanged(nameof(ConnectionState));
            }
        }
        #endregion Property - ConnectionState

        #endregion Properties

        private Address _address;

        private bool _isStopRequested;
        private Thread _thread;
        private Dispatcher _dispatcher;

        private ConcurrentQueue<QueuedPacket> _packetQueue;

        public void Connect(string hostname, ushort port)
        {
            // Argument Validation //
            if (string.IsNullOrWhiteSpace(hostname))
                throw new ArgumentException($"The argument '{nameof(hostname)}' cannot be null, empty, or whitespace.", nameof(hostname));

            // State Validation //
            if (IsRunning)
                throw new InvalidOperationException("Cannot connect Peer to a Host, Peer is already running.");

            // Argument Storing //
            _address = new Address() { Port = port };
            _address.SetHost(hostname);

            // Variable Initialization //
            IsRunning = true;
            _isStopRequested = false;
            _dispatcher = Dispatcher.CurrentDispatcher;
            _packetQueue = new ConcurrentQueue<QueuedPacket>();

            // Thread Creation //
            _thread = new Thread(new ThreadStart(Run));
            _thread.Start();
        }

        public void Disconnect()
        {
            // State Validation //
            if (!IsRunning)
                throw new InvalidOperationException("Cannot disconnect Peer, Peer is not running.");

            // Stop Internal Thread //
            _isStopRequested = true;
            _thread.Join();
        }

        public void SendPacket(byte channelID, byte[] data)
        {
            // State Validation //
            if (!IsRunning)
                throw new InvalidOperationException("Cannot send packet, Client is not currently running.");
            if (ConnectionState != IPeerService.ConnectionStates.Connected)
                throw new InvalidOperationException("Cannot send packet, Client is not connected.");

            // Enqueue Packet //
            _packetQueue.Enqueue(new QueuedPacket(channelID, data));
        }

        private void Run()
        {
            // Create and initialize ENet Host (As a client)
            using Host client = new Host();
            client.Create();

            // NOTE: Peer is not officially connected until a Connect event is received.
            var peer = client.Connect(_address, MAX_CHANNELS);
            _dispatcher.BeginInvoke(() => ConnectionState = IPeerService.ConnectionStates.Connecting);

            // Internal loop
            var isLoopRunning = true;
            while (isLoopRunning)
            {
                // Stop Requested //
                if (_isStopRequested)
                {
                    isLoopRunning = false;

                    // State is Disconnected.
                    var lastState = ConnectionState;
                    _dispatcher.BeginInvoke(() => ConnectionState = IPeerService.ConnectionStates.Disconnected);
                    // Connection was Closed.
                    if (lastState == IPeerService.ConnectionStates.Connected)
                    {
                        _dispatcher.BeginInvoke(() => OnConnectionClosed(_address.GetHost(), _address.Port));
                        peer.Disconnect(0);
                    }
                    // Connection was Canceled.
                    else if (lastState == IPeerService.ConnectionStates.Connecting)
                        _dispatcher.BeginInvoke(() => OnConnectionCanceled(_address.GetHost(), _address.Port));

                    continue;
                }

                // Send Queued Packets //
                for (int i = 0; i < Math.Min(_packetQueue.Count, MAX_PACKETS_PER_TICK); i++)
                {
                    // Dequeue the next QueuedPacket
                    if (!_packetQueue.TryDequeue(out QueuedPacket queuedPacket))
                        break;

                    // Send the QueuedPacket
                    if (!TrySendPacket(peer, queuedPacket, out string errorMessage))
                        _dispatcher.BeginInvoke(() =>
                            OnPacketSendFailed(queuedPacket.ChannelID, queuedPacket.Data, errorMessage));
                }

                // Service Client //
                if (client.Service(0, out Event netEvent) == 1)
                {
                    switch(netEvent.Type)
                    {
                        // Connect - Triggered when connection to the server was successful.
                        case EventType.Connect:
                            // Connection was Successful: State is Connected.
                            _dispatcher.BeginInvoke(() => ConnectionState = IPeerService.ConnectionStates.Connected);
                            _dispatcher.BeginInvoke(() => OnConnectionSuccessful(_address.GetHost(), _address.Port));
                            break;

                        // Disconnect - Triggered when the connection was closed by the server.
                        case EventType.Disconnect:
                            // Connection was Closed: State is Disconnected.
                            _dispatcher.BeginInvoke(() => ConnectionState = IPeerService.ConnectionStates.Disconnected);
                            _dispatcher.BeginInvoke(() => OnConnectionClosed(_address.GetHost(), _address.Port));
                            // Stop Loop.
                            isLoopRunning = false;
                            break;

                        // Timeout - Triggered by an unsuccessful or lost connection.
                        case EventType.Timeout:
                            // State is Timedout.
                            var lastState = ConnectionState;
                            _dispatcher.BeginInvoke(() => ConnectionState = IPeerService.ConnectionStates.Timedout);
                            // Connection was Lost.
                            if (lastState == IPeerService.ConnectionStates.Connected)
                                _dispatcher.BeginInvoke(() => OnConnectionLost(_address.GetHost(), _address.Port));
                            // Connection was Unsuccessful.
                            else if (lastState == IPeerService.ConnectionStates.Connecting)
                                _dispatcher.BeginInvoke(() => OnConnectionUnsuccessful(_address.GetHost(), _address.Port));
                            // Stop Loop.
                            isLoopRunning = false;
                            break;

                        // Receive - Triggered when data was received from the connected server.
                        case EventType.Receive:
                            byte[] data = new byte[netEvent.Packet.Length];
                            netEvent.Packet.CopyTo(data);
                            netEvent.Packet.Dispose();
                            _dispatcher.BeginInvoke(() => OnDataReceived(netEvent.ChannelID, data));
                            break;
                    }
                }

                // Sleep the thread to give the CPU a chance to chill lol.
                Thread.Sleep(1);
            }

            // Send any remaining packets
            client.Flush();

            // Cleanup //
            _dispatcher.BeginInvoke(PostThreadCleanup);
        }

        private void PostThreadCleanup()
        {
            _thread = null;
            _address = default;
            IsRunning = false;
            _dispatcher = null;
            _packetQueue.Clear();
            _packetQueue = null;
        }

        private bool TrySendPacket(Peer peer, QueuedPacket queuedPacket, out string errorMessage)
        {
            var channelID = queuedPacket.ChannelID;

            errorMessage = null;

            // Ensure ChannelID is valid
            if (channelID < 0 || channelID >= MAX_CHANNELS)
                errorMessage = $"Invalid {nameof(QueuedPacket.ChannelID)}. Was {channelID}, expected between 0 and {MAX_CHANNELS}(exc)";

            // If an error occured, skip this packet and report the error
            if (errorMessage != null)
                return false;

            // Send packet to peer.
            var packet = default(Packet);
            packet.Create(queuedPacket.Data);
            peer.Send(channelID, ref packet);

            // Return true for success
            return true;
        }


        #region Callbacks

        private void OnConnectionSuccessful(string hostname, ushort port)
        {
            ConnectionSuccessful?.Invoke(this, new IPeerService.ConnectionEventArgs()
                { Hostname = hostname, Port=port });
        }

        private void OnConnectionUnsuccessful(string hostname, ushort port)
        {
            ConnectionUnsuccessful?.Invoke(this, new IPeerService.ConnectionEventArgs()
                { Hostname = hostname, Port = port });
        }

        private void OnConnectionLost(string hostname, ushort port)
        {
            ConnectionLost?.Invoke(this, new IPeerService.ConnectionEventArgs()
                { Hostname = hostname, Port = port });
        }

        private void OnConnectionCanceled(string hostname, ushort port)
        {
            ConnectionCanceled?.Invoke(this, new IPeerService.ConnectionEventArgs()
                { Hostname = hostname, Port = port });
        }

        private void OnConnectionClosed(string hostname, ushort port)
        {
            ConnectionClosed?.Invoke(this, new IPeerService.ConnectionEventArgs()
                { Hostname = hostname, Port = port });
        }

        private void OnDataReceived(byte channelID, byte[] data)
        {
            DataReceived?.Invoke(this, new IPeerService.DataReceivedEventArgs()
            { ChannelID=channelID, Data=data });
        }

        private void OnPacketSendFailed(byte channelID, byte[] data, string errorMessage)
        {
            PacketSendFailed?.Invoke(this, new IPeerService.PacketSendFailedEventArgs()
            { ChannelID=channelID, Data=data, ErrorMessage=errorMessage });
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
            public byte ChannelID { get; }
            public byte[] Data { get; }

            public QueuedPacket(byte channelID, byte[] data)
            {
                ChannelID = channelID;
                Data = new byte[data.Length];
                Array.Copy(data, Data, data.Length);
            }
        }
    }
}
