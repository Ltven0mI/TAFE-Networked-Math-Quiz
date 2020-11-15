using BinaryTree;
using CommonClasses;
using CommonClasses.Networking;
using CommonClasses.Utilities;
using MVVMUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using TeacherApp.Services.Interfaces;
using TeacherApp.ViewModels.Interfaces;
using Unity.Injection;

namespace TeacherApp.ViewModels
{
    public class MainWindowVM : ViewModelBase, IViewMainWindowVM
    {
        public const ushort NETWORKING_PORT = 62311;

        #region Properties

        #region Property - LinkedListText
        private string _linkedListText;
        public string LinkedListText
        {
            get => _linkedListText;
            private set
            {
                _linkedListText = value;
                RaisePropertyChanged(nameof(LinkedListText));
            }
        }
        #endregion Property - LinkedListText

        #region Property - NetworkStatus
        private string _networkStatus;
        public string NetworkStatus
        {
            get => _networkStatus;
            private set
            {
                _networkStatus = value;
                RaisePropertyChanged(nameof(NetworkStatus));
            }
        }
        #endregion Property - NetworkStatus

        #endregion Properties

        #region Commands

        #region Command - DisplayLinkedListCommand
        private RelayCommand _displayLinkedListCommand;
        public ICommand DisplayLinkedListCommand
        {
            get => _displayLinkedListCommand ??=
                new RelayCommand(
                    _ =>
                    {
                        DisplayLinkedList();
                    }
                );
        }
        #endregion Command - DisplayLinkedListCommand

        #region Command - ExitCommand
        private RelayCommand _exitCommand;
        public ICommand ExitCommand
        {
            get => _exitCommand ??=
                new RelayCommand(
                    closable =>
                    {
                        // Prompt User to Confirm //
                        if (MessageBox.Show("Are you sure you wish to exit?", "Exit", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            return;

                        if (HostService.IsRunning)
                            StopServer();
                        ((IClosable)closable).Close();
                    }
                );
        }
        #endregion Command - ExitCommand

        #region Command - HostCommand
        private RelayCommand _hostCommand;
        public ICommand HostCommand
        {
            get => _hostCommand ??=
                new RelayCommand(
                    _ => StartServer(),
                    _ => !HostService.IsRunning
                );
        }
        #endregion Command - HostCommand

        #region Command - ShutdownCommand
        private RelayCommand _shutdownCommand;
        public ICommand ShutdownCommand
        {
            get => _shutdownCommand ??=
                new RelayCommand(
                    _ => StopServer(),
                    _ => HostService.IsRunning
                );
        }
        #endregion Command - ShutdownCommand

        #endregion Commands

        #region Models
        #endregion Models

        #region ViewModels

        [Unity.Dependency]
        public IViewQuestionInputVM QuestionInputVM { get; set; }

        [Unity.Dependency]
        public IViewQuestionTableVM QuestionTableVM { get; set; }

        [Unity.Dependency]
        public IViewBinaryTreeVM BinaryTreeVM { get; set; }

        #endregion ViewModels

        #region Services

        #region Service - HostService
        [Unity.Dependency]
        public IHostService HostService
        {
            get => _hostService;
            set
            {
                // Deregister callbacks
                if (_hostService != null)
                {
                    _hostService.ClientConnected -= HostService_ClientConnected;
                    _hostService.ClientDisconnected -= HostService_ClientDisconnected;
                    _hostService.ClientTimedout -= HostService_ClientTimedout;
                    _hostService.PacketSendFailed -= HostService_PacketSendFailed;
                }

                _hostService = value;

                // Register callbacks
                if (_hostService != null)
                {
                    _hostService.ClientConnected += HostService_ClientConnected;
                    _hostService.ClientDisconnected += HostService_ClientDisconnected;
                    _hostService.ClientTimedout += HostService_ClientTimedout;
                    _hostService.PacketSendFailed += HostService_PacketSendFailed;
                }
            }
        }
        private IHostService _hostService;
        #endregion Service - HostService

        #region Service - MathQuestionRepositoryService
        [Unity.Dependency]
        public IMathQuestionRepositoryService MathQuestionRepositoryService
        {
            get => _mathQuestionRepositoryService;
            set
            {
                // Deregister Callbacks //
                if (_mathQuestionRepositoryService != null)
                    _mathQuestionRepositoryService.ActiveQuestionAnswered -= MathQuestionRepositoryService_ActiveQuestionAnswered;

                _mathQuestionRepositoryService = value;

                // Register Callbacks //
                if (_mathQuestionRepositoryService != null)
                    _mathQuestionRepositoryService.ActiveQuestionAnswered += MathQuestionRepositoryService_ActiveQuestionAnswered;
            }
        }
        private IMathQuestionRepositoryService _mathQuestionRepositoryService;
        #endregion Service - MathQuestionRepositoryService

        #endregion Services

        // Constructors //
        public MainWindowVM()
        {

        }

        private void StartServer()
        {
            // State Validation //
            if (HostService.IsRunning)
                throw new InvalidOperationException("Cannot start server when server is already running.");

            NetworkStatus = "Awaiting connection";
            HostService.Start(NETWORKING_PORT);
        }

        private void StopServer()
        {
            // State Validation //
            if (!HostService.IsRunning)
                throw new InvalidOperationException("Cannot stop server when server is not running.");

            HostService.Shutdown();
            NetworkStatus = "Host shutdown";
        }

        /// <summary>
        /// Display all incorrectly answered <see cref="MathQuestion"/>(s).
        /// </summary>
        private void DisplayLinkedList()
        {
            // IF: No Incorrectly Answered Questions //
            if (MathQuestionRepositoryService.IncorrectQuestions.Count == 0)
            {
                LinkedListText = "There are no incorrectly answered questions to display.";
                return;
            }

            // Display Incorrectly Answered Questions //
            var enumeratedText = string.Join(" <-> ", MathQuestionRepositoryService.IncorrectQuestions);
            LinkedListText = $"HEAD <-> {enumeratedText} <-> TAIL";
        }


        #region Callback Methods


        #region Source Group - HostService

        private void HostService_ClientConnected(object sender, IHostService.ClientConnectionEventArgs e)
        {
            NetworkStatus = "Connected";
        }
        private void HostService_ClientDisconnected(object sender, IHostService.ClientConnectionEventArgs e)
        {
            NetworkStatus = "Awaiting connection";
        }
        private void HostService_ClientTimedout(object sender, IHostService.ClientConnectionEventArgs e)
        {
            NetworkStatus = "Awaiting connection";
        }

        private void HostService_PacketSendFailed(object sender, IHostService.PacketSendFailedEventArgs e)
        {
            MessageBox.Show($"Failed to send packet: {e.ErrorMessage}");
        }

        #endregion Source Group - HostService


        #region Source Group - MathQuestionRepositoryService

        private void MathQuestionRepositoryService_ActiveQuestionAnswered(object sender, bool wasCorrect)
        {
            // Display Result //
            LinkedListText = $"Student answered {(wasCorrect ? "correctly" : "incorrectly")}!";
        }

        #endregion Source Group - MathQuestionRepositoryService


        #endregion Callback Methods
    }
}
