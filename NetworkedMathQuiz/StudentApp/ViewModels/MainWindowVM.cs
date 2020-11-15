using CommonClasses;
using CommonClasses.Networking;
using MVVMUtil;
using StudentApp.Models;
using StudentApp.Services.Interfaces;
using StudentApp.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Unity.Injection;

namespace StudentApp.ViewModels
{
    public class MainWindowVM : ViewModelBase, IViewMainWindowVM
    {
        private const ushort NETWORK_PORT = 62311;

        #region Properties

        #region Property - QuestionText
        private string _questionText;
        public string QuestionText { get => _questionText; }
        private void UpdateQuestionText()
        {
            _questionText = QuestionModel.ActiveQuestion?.ToQuestionString();
            RaisePropertyChanged(nameof(QuestionText));
        }
        #endregion Property - QuestionText

        #region Property - UserAnswer
        private string _userAnswer;
        public string UserAnswer
        {
            get => _userAnswer;
            set
            {
                if (double.TryParse(value, out double d))
                    value = Math.Round(d, MathQuestion.NUMBER_SCALE).ToString();

                _userAnswer = value?.Trim();
                RaisePropertyChanged(nameof(UserAnswer));
            }
        }
        #endregion Property - UserAnswer

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

        #region Property - IsAwaitingResult
        private bool _isAwaitingResult;
        public bool IsAwaitingResult
        {
            get => _isAwaitingResult;
            private set
            {
                _isAwaitingResult = value;
                RaisePropertyChanged(nameof(IsAwaitingResult));
                UpdateCanSubmitAnswer();
            }
        }
        #endregion Property - IsAwaitingResult

        #region Property - CanSubmitAnswer
        private bool _canSubmitAnswer;
        public bool CanSubmitAnswer { get => _canSubmitAnswer; }
        private void UpdateCanSubmitAnswer()
        {
            _canSubmitAnswer = IsConnected && QuestionModel.ActiveQuestion != null && !IsAwaitingResult;
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
                    _ =>
                    {
                        double answer = default;

                        // Gather Input Errors //
                        var errors = new List<string>();

                        // Validate FirstNumber //
                        if (string.IsNullOrWhiteSpace(UserAnswer))
                            errors.Add("Answer is missing");
                        else if (!double.TryParse(UserAnswer, out answer))
                            errors.Add("Answer is not numeric");

                        // IF: Input errors exist //
                        if (errors.Count > 0)
                        {
                            MessageBox.Show(string.Join(Environment.NewLine, errors), $"{errors.Count} error(s) detected!");
                            return;
                        }

                        SubmitAnswer(answer);
                    },
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
                        // Prompt User to Confirm //
                        if (MessageBox.Show("Are you sure you wish to exit?", "Exit", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                            return;

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

        #region Models

        #region Model - QuestionModel
        private QuestionModel _questionModel;
        public QuestionModel QuestionModel
        {
            get
            {
                if (_questionModel == null)
                {
                    _questionModel = new QuestionModel();
                    _questionModel.PropertyChanged += QuestionModel_PropertyChanged;
                }
                return _questionModel;
            }
        }
        #endregion Model - QuesitonModel

        #endregion Models

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
                    _peerService.DataReceived -= PeerService_DataReceived;
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
                    _peerService.DataReceived += PeerService_DataReceived;
                }
            }
        }
        private IPeerService _peerService;
        #endregion Service - PeerService

        #endregion Services

        /// <summary>
        /// Connect to <c>'localhost'</c>.
        /// </summary>
        /// <exception cref="InvalidOperationException">when the peer is already running.</exception>
        private void ConnectToLocalHost()
        {
            // State Validation //
            if (PeerService.IsRunning == true)
                throw new InvalidOperationException("Cannot connect to localhost: peer is already running.");

            PeerService.Connect("localhost", NETWORK_PORT);
        }

        /// <summary>
        /// Disconnect from the current host.
        /// </summary>
        /// <exception cref="InvalidOperationException">when the peer is not running.</exception>
        private void DisconnectFromHost()
        {
            // State Validation //
            if (PeerService.IsRunning == false)
                throw new InvalidOperationException("Cannot disconnect from localhost: peer is not running.");

            PeerService.Disconnect();
        }

        /// <summary>
        /// Submit <paramref name="answer"/> as the answer for the current <see cref="MathQuestion"/>.
        /// </summary>
        /// <param name="answer">The answer.</param>
        /// <exception cref="InvalidOperationException">when there is no <see cref="MathQuestion"/> awaiting an answer.</exception>
        private void SubmitAnswer(double answer)
        {
            // State Validation //
            if (QuestionModel.ActiveQuestion == null)
                throw new InvalidOperationException("No question awaiting an answer.");

            // Construct, serialize, and send packet
            var packet = new AnswerPacket(answer);
            var bytes = packet.Serialize();
            PeerService.SendPacket(0, bytes);

            IsAwaitingResult = true;
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
            QuestionModel.ActiveQuestion = null;
        }
        private void PeerService_ConnectionLost(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = false;
            IsAwaitingResult = false;
            UserAnswer = null;
            QuestionModel.ActiveQuestion = null;
        }
        private void PeerService_ConnectionCanceled(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = false;
            IsAwaitingResult = false;
            UserAnswer = null;
            QuestionModel.ActiveQuestion = null;
        }
        private void PeerService_ConnectionClosed(object sender, IPeerService.ConnectionEventArgs e)
        {
            IsConnected = false;
            IsAwaitingResult = false;
            UserAnswer = null;
            QuestionModel.ActiveQuestion = null;
        }

        private void PeerService_DataReceived(object sender, IPeerService.DataReceivedEventArgs e)
        {
            var packetType = SPacketBase.ValidateHeader(e.Data);

            // Create packet instance and deserialize.
            var constructor = packetType.GetConstructor(new Type[0]);
            var packet = (SPacketBase)constructor.Invoke(null);
            packet.Deserialize(e.Data);

            // Call appropriate received callback.
            if (packetType == typeof(QuestionPacket))
                OnReceivedQuestionPacket((QuestionPacket)packet);
            else if (packetType == typeof(ResultPacket))
                OnReceivedResultPacket((ResultPacket)packet);
        }

        #endregion Source Group - PeerService

        #region Source Group - QuestionModel

        private void QuestionModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                // QuestionModel.ActiveQuestion Changed //
                case nameof(QuestionModel.ActiveQuestion):
                    UpdateQuestionText();
                    UpdateCanSubmitAnswer();
                    break;
            }
        }
        
        #endregion Source Group - QuestionModel

        private void OnReceivedQuestionPacket(QuestionPacket packet)
        {
            QuestionModel.ActiveQuestion = new MathQuestion(packet.LeftOperand, packet.RightOperand, packet.Operator);
        }

        private void OnReceivedResultPacket(ResultPacket packet)
        {
            IsAwaitingResult = false;
            QuestionModel.ActiveQuestion = null;
            UserAnswer = null;

            // Display result to user.
            var msg = (packet.WasCorrect) ? "Correct!" : "Incorrect!";
            var title = (packet.WasCorrect) ? "Correct Answer" : "Incorrect Answer";
            MessageBox.Show(msg, title);
        }

        #endregion Callback Methods
    }
}
