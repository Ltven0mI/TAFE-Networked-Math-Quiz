using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TeacherApp.Services.Interfaces
{
    /**********************************************************/
    // Filename:   IHostService.cs
    // Purpose:    A service intended to provide an asynchronous
    //             - method of hosting a simple server.
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
    /// A service intended to provide an asynchronous method of hosting a simple server.
    /// </summary>
    public interface IHostService : INotifyPropertyChanged
    {
        #region Events

        #region Event - ClientConnected, ClientDisconnected, ClientTimedout
        public class ClientConnectionEventArgs { public uint PeerID { get; set; } }
        public delegate void ClientConnectionEventHandler(object sender, ClientConnectionEventArgs e);
        public event ClientConnectionEventHandler ClientConnected;
        public event ClientConnectionEventHandler ClientDisconnected;
        public event ClientConnectionEventHandler ClientTimedout;
        #endregion Event - ClientConnected, ClientDisconnected, ClientTimedout

        #region Event - DataReceived
        public class DataReceivedEventArgs
        {
            public uint PeerID { get; set; }
            public byte ChannelID { get; set; }
            public byte[] Data { get; set; }
        }
        public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);
        public event DataReceivedEventHandler DataReceived;
        #endregion Event - DataReceived

        #region Event - PacketSendFailed
        public class PacketSendFailedEventArgs
        {
            public uint PeerID { get; set; }
            public byte ChannelID { get; set; }
            public byte[] Data { get; set; }
            public string ErrorMessage { get; set; }
        }
        public delegate void PacketSendFailedEventHandler(object sender, PacketSendFailedEventArgs e);
        public event PacketSendFailedEventHandler PacketSendFailed;
        #endregion Event - PacketSendFailed

        #region Event - PacketBroadcastFailed
        public class PacketBroadcastFailedEventArgs
        {
            public byte ChannelID { get; set; }
            public byte[] Data { get; set; }
            public string ErrorMessage { get; set; }
        }
        public delegate void PacketBroadcastFailedEventHandler(object sender, PacketBroadcastFailedEventArgs e);
        public event PacketBroadcastFailedEventHandler PacketBroadcastFailed;
        #endregion Event - PacketBroadcastFailed

        #endregion Events

        public bool IsRunning { get; }

        public void Start(ushort port);
        public void Shutdown();
        public void SendPacket(uint peerID, byte channelID, byte[] data);
        public void BroadcastPacket(byte channelID, byte[] data);
    }
}
