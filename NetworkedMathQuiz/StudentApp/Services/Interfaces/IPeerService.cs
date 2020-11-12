using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StudentApp.Services.Interfaces
{
    public interface IPeerService : INotifyPropertyChanged
    {
        #region Events

        #region Event Group - Connection
        public class ConnectionEventArgs
        {
            public string Hostname { get; set; }
            public ushort Port { get; set; }
        }
        public delegate void ConnectionEventHandler(object sender, ConnectionEventArgs e);
        public event ConnectionEventHandler ConnectionSuccessful;
        public event ConnectionEventHandler ConnectionUnsuccessful;
        public event ConnectionEventHandler ConnectionLost;
        public event ConnectionEventHandler ConnectionCanceled;
        public event ConnectionEventHandler ConnectionClosed;
        #endregion Event Group - Connection

        #region Event - DataReceived
        public class DataReceivedEventArgs
        {
            public byte ChannelID { get; set; }
            public byte[] Data { get; set; }
        }
        public delegate void DataReceivedEventHandler(object sender, DataReceivedEventArgs e);
        public event DataReceivedEventHandler DataReceived;
        #endregion Event - DataReceived

        #region Event - PacketSendFailed
        public class PacketSendFailedEventArgs
        {
            public byte ChannelID { get; set; }
            public byte[] Data { get; set; }
            public string ErrorMessage { get; set; }
        }
        public delegate void PacketSendFailedEventHandler(object sender, PacketSendFailedEventArgs e);
        public event PacketSendFailedEventHandler PacketSendFailed;
        #endregion Event - PacketSendFailed

        #endregion Events

        public enum ConnectionStates { Disconnected, Connecting, Connected, Timedout };

        public bool IsRunning { get; }
        public ConnectionStates ConnectionState { get; }
        public void Connect(string hostname, ushort port);
        public void Disconnect();
        public void SendPacket(byte channelID, byte[] data);
    }
}
