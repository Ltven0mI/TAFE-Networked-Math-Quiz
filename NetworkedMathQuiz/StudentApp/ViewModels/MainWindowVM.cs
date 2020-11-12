using MVVMUtil;
using StudentApp.Services.Interfaces;
using StudentApp.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace StudentApp.ViewModels
{
    public class MainWindowVM : ViewModelBase, IViewMainWindowVM
    {
        private const ushort NETWORK_PORT = 62311;

        #region Properties

        #region Property - IsConnected
        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                RaisePropertyChanged(nameof(IsConnected));
                UpdateCanSubmitAnswer();
            }
        }
        #endregion Property - IsConnected

        #region Property - HasPendingQuestion
        private bool _hasPendingQuestion;
        public bool HasPendingQuestion
        {
            get => _hasPendingQuestion;
            set
            {
                _hasPendingQuestion = value;
                RaisePropertyChanged(nameof(HasPendingQuestion));
                UpdateCanSubmitAnswer();
            }
        }
        #endregion Property - HasPendingQuestion

        #region Property - CanSubmitAnswer
        public bool CanSubmitAnswer { get; private set; }
        private void UpdateCanSubmitAnswer()
        {
            CanSubmitAnswer = IsConnected && HasPendingQuestion;
            RaisePropertyChanged(nameof(CanSubmitAnswer));
        }
        #endregion Property - CanSubmitAnswer

        #endregion Properties

        #region Commands

        #region Command - SubmitCommand
        private RelayCommand _submitCommand;
        public ICommand SubmitCommand
        {
            get => _submitCommand ??=
                new RelayCommand(
                    _ => MessageBox.Show("Submit..."),
                    _ => CanSubmitAnswer
                );
        }
        #endregion Command - SubmitCommand

        #region Command - ExitCommand
        private RelayCommand _exitCommand;
        public ICommand ExitCommand
        {
            get => _exitCommand ??=
                new RelayCommand(
                    closable =>
                    {
                        if (PeerService.IsRunning)
                            DisconnectFromHost();
                        ((IClosable)closable).Close();
                    }
                );
        }
        #endregion Command - ExitCommand

        #region Command - ConnectCommand
        private RelayCommand _connectCommand;
        public ICommand ConnectCommand
        {
            get => _connectCommand ??=
                new RelayCommand(
                    _ => ConnectToLocalHost(),
                    _ => !PeerService.IsRunning
                );
        }
        #endregion Command - ConnectCommand

        #region Command - DisconnectCommand
        private RelayCommand _disconnectCommand;
        public ICommand DisconnectCommand
        {
            get => _disconnectCommand ??=
                new RelayCommand(
                    _ => DisconnectFromHost(),
                    _ => PeerService.IsRunning
                );
        }
        #endregion Command - DisconnectCommand

        #endregion Commands

        #region ViewModels
        #endregion ViewModels

        #region Services

        #region Service - PeerService
        [Unity.Dependency]
        public IPeerService PeerService
        {
            get => _peerService;
            set
            {
                // Deregister callbacks.
                if (_peerService != null)
                {
                    _peerService.ConnectionSuccessful -= PeerService_ConnectionSuccesful;
                    _peerService.ConnectionUnsuccessful -= PeerService_ConnectionUnsuccesful;
                    _peerService.ConnectionLost -= PeerService_ConnectionLost;
                    _peerService.ConnectionCanceled -= PeerService_ConnectionCanceled;
                    _peerService.ConnectionClosed -= PeerService_ConnectionClosed;
                }

                _peerService = value;

                // Register callbacks.
                if (_peerService != null)
                {
                    _peerService.ConnectionSuccessful += PeerService_ConnectionSuccesful;
                    _peerService.ConnectionUnsuccessful += PeerService_ConnectionUnsuccesful;
                    _peerService.ConnectionLost += PeerService_ConnectionLost;
                    _peerService.ConnectionCanceled += PeerService_ConnectionCanceled;
                    _peerService.ConnectionClosed += PeerService_ConnectionClosed;
                }
            }
        }
        private IPeerService _peerService;
        #endregion Service - PeerService

        #endregion Services

        public MainWindowVM()
        {

        }

        private void ConnectToLocalHost()
        {
            // State Validation //
            if (PeerService.IsRunning == true)
                throw new InvalidOperationException("Cannot connect to localhost: peer is already running.");

            PeerService.Connect("localhost", NETWORK_PORT);
            MessageBox.Show("Connect");
        }

        private void DisconnectFromHost()
        {
            // State Validation //
            if (PeerService.IsRunning == false)
                throw new InvalidOperationException("Cannot disconnect from localhost: peer is not running.");

            PeerService.Disconnect();
            MessageBox.Show("Disconnect");
        }

        #region Callback Methods


        #region Source Group - PeerService

        private void PeerService_ConnectionSuccesful(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = true;
        }
        private void PeerService_ConnectionUnsuccesful(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = false;
        }
        private void PeerService_ConnectionLost(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = false;
        }
        private void PeerService_ConnectionCanceled(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = false;
        }
        private void PeerService_ConnectionClosed(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = false;
        }

        #endregion Source Group - PeerService


        #endregion Callback Methods
    }
}
