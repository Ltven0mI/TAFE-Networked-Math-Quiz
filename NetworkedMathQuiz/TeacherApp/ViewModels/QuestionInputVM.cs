using CommonClasses;
using CommonClasses.Networking;
using MVVMUtil;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TeacherApp.Services.Interfaces;
using TeacherApp.ViewModels.Interfaces;

namespace TeacherApp.ViewModels
{
    public class QuestionInputVM : ViewModelBase, IViewQuestionInputVM
    {
        #region Properties

        #region Property - FirstNumberString
        private string _firstNumberString;
        public string FirstNumberString
        {
            get => _firstNumberString;
            set
            {
                if (double.TryParse(value, out double d))
                    value = Math.Round(d, MathQuestion.NUMBER_SCALE).ToString();

                _firstNumberString = value?.Trim();
                RaisePropertyChanged(nameof(FirstNumberString));
            }
        }
        #endregion Property - FirstNumberString

        #region Property - SelectedOperatorIndex
        private int _selectedOperatorIndex;
        public int SelectedOperatorIndex
        {
            get => _selectedOperatorIndex;
            set
            {
                _selectedOperatorIndex = value;
                RaisePropertyChanged(nameof(SelectedOperatorIndex));
            }
        }
        #endregion Property - SelectedOperatorIndex

        #region Property - SecondNumber
        private string _secondNumberString;
        public string SecondNumberString
        {
            get => _secondNumberString;
            set
            {
                if (double.TryParse(value, out double d))
                    value = Math.Round(d, MathQuestion.NUMBER_SCALE).ToString();

                _secondNumberString = value?.Trim();
                RaisePropertyChanged(nameof(SecondNumberString));
            }
        }
        #endregion Property - SecondNumber

        #region Property - AnswerString
        private string _answerString;
        public string AnswerString
        {
            get => _answerString;
            private set
            {
                _answerString = value;
                RaisePropertyChanged(nameof(AnswerString));
            }
        }
        #endregion Property - AnswerString

        #region Property - IsPendingAnswer
        private bool _isPendingAnswer;
        public bool IsPendingAnswer
        {
            get => _isPendingAnswer;
            private set
            {
                _isPendingAnswer = value;
                RaisePropertyChanged(nameof(IsPendingAnswer));
                UpdateCanSendQuestion();
            }
        }
        #endregion Property - IsPendingAnswer

        #region Property - IsStudentConnected
        private bool _isStudentConnected;
        public bool IsStudentConnected
        {
            get => _isStudentConnected;
            private set
            {
                _isStudentConnected = value;
                RaisePropertyChanged(nameof(IsStudentConnected));
                UpdateCanSendQuestion();
            }
        }
        #endregion Property - IsStudentConnected

        #region Property - CanSendQuestion
        private bool _canSendQuestion;
        public bool CanSendQuestion { get => _canSendQuestion; }
        private void UpdateCanSendQuestion()
        {
            _canSendQuestion = IsStudentConnected && !IsPendingAnswer;
            RaisePropertyChanged(nameof(CanSendQuestion));
        }
        #endregion Property - CanSendQuestion

        #endregion Properties


        #region Commands

        #region Command - SendCommand
        private RelayCommand _sendCommand;
        public ICommand SendCommand
        {
            get => _sendCommand ??=
                new RelayCommand(
                    _ =>
                    {
                        double firstNumber = default;
                        double secondNumber = default;

                        // Gather Input Errors //
                        var errors = new List<string>();

                        // Validate FirstNumber //
                        if (string.IsNullOrWhiteSpace(FirstNumberString))
                            errors.Add("First number is missing");
                        else if (!double.TryParse(FirstNumberString, out firstNumber))
                            errors.Add("First number is not numeric");

                        // Validate SecondNumber //
                        if (string.IsNullOrWhiteSpace(SecondNumberString))
                            errors.Add("Second number is missing");
                        else if (!double.TryParse(SecondNumberString, out secondNumber))
                            errors.Add("Second number is not numeric");

                        // IF: Input errors exist //
                        if (errors.Count > 0)
                        {
                            MessageBox.Show(string.Join(Environment.NewLine, errors), $"{errors.Count} error(s) detected!");
                            return;
                        }

                        // Construct Question //
                        var mathQuestion = new MathQuestion(firstNumber, secondNumber, (MathOperator)SelectedOperatorIndex);

                        // IF: Question Already Exists //
                        if (MathQuestionRepositoryService.DoesQuestionExist(mathQuestion))
                        {
                            MessageBox.Show($"This question has already been asked.{Environment.NewLine}" +
                                "Please enter a different question.", "Duplicate Question");
                            return;
                        }

                        // Send Question //
                        SendQuestion(mathQuestion);
                    },
                    _ =>
                    {
                        return CanSendQuestion;
                    }
                );
        }
        #endregion Command - SendCommand

        #endregion Commands


        #region Services

        #region Service - HostService
        [Unity.Dependency]
        public IHostService HostService
        {
            get => _hostService;
            set
            {
                // Deregister Callbacks //
                if (_hostService != null)
                {
                    _hostService.ClientConnected -= HostService_ClientConnected;
                    _hostService.ClientTimedout -= HostService_ClientTimedout;
                    _hostService.ClientDisconnected -= HostService_ClientDisconnected;
                    _hostService.DataReceived -= HostService_DataReceived;
                }

                _hostService = value;

                // Register Callbacks //
                if (_hostService != null)
                {
                    _hostService.ClientConnected += HostService_ClientConnected;
                    _hostService.ClientTimedout += HostService_ClientTimedout;
                    _hostService.ClientDisconnected += HostService_ClientDisconnected;
                    _hostService.DataReceived += HostService_DataReceived;
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
                    _mathQuestionRepositoryService.ActiveQuestionChanged -= MathQuestionRepositoryService_ActiveQuestionChanged;

                _mathQuestionRepositoryService = value;

                // Register Callbacks //
                if (_mathQuestionRepositoryService != null)
                    _mathQuestionRepositoryService.ActiveQuestionChanged += MathQuestionRepositoryService_ActiveQuestionChanged;
            }
        }
        private IMathQuestionRepositoryService _mathQuestionRepositoryService;
        #endregion Service - MathQuestionRepositoryService

        #endregion Services


        /// <summary>
        /// Sends <paramref name="question"/> to the connected student.
        /// </summary>
        /// <param name="question">The <see cref="MathQuestion"/> to send.</param>
        private void SendQuestion(MathQuestion question)
        {
            // State Validation //
            if (IsPendingAnswer == true)
                throw new InvalidOperationException("Cannot send a question while there is already an active question.");
            if (IsStudentConnected == false)
                throw new InvalidOperationException("Cannot send a question when no student is connected.");
            if (MathQuestionRepositoryService.DoesQuestionExist(question))
                throw new InvalidOperationException($"The math question ({question}) has already been asked.");

            AnswerString = $"{question.Answer}";
            MathQuestionRepositoryService.AskQuestion(question);

            // Construct, serialize, and send packet
            var packet = new QuestionPacket(question.LeftOperand, question.RightOperand, question.Operator);
            var bytes = packet.Serialize();
            HostService.SendPacket(0, 0, bytes);
        }

        /// <summary>
        /// Sends the result of the students answer, to the student.<br/>
        /// I.e. Is correct or Is NOT correct.
        /// </summary>
        /// <param name="wasCorrect">The result to send to the student.</param>
        private void SendResult(bool wasCorrect)
        {
            // State Validation //
            if (IsStudentConnected == false)
                throw new InvalidOperationException("Cannot send result when no student is connected.");

            // Construct, serialize, and send packet
            var packet = new ResultPacket(wasCorrect);
            var bytes = packet.Serialize();
            HostService.SendPacket(0, 0, bytes);
        }

        /// <summary>
        /// Clears the input fields to their default values.
        /// </summary>
        private void ClearFields()
        {
            FirstNumberString = default;
            SelectedOperatorIndex = 0;
            SecondNumberString = default;
            AnswerString = "";
        }


        #region Callbacks


        #region Source Group - HostService

        private void HostService_ClientConnected(object sender, IHostService.ClientConnectionEventArgs e)
        {
            IsStudentConnected = true;
        }
        private void HostService_ClientDisconnected(object sender, IHostService.ClientConnectionEventArgs e)
        {
            IsStudentConnected = false;

            // Student left... The question won't ever be answered now...
            // Clean up in preperation for next student.
            MathQuestionRepositoryService.ClearActiveQuestion();

            // Clear input fields
            ClearFields();
        }
        private void HostService_ClientTimedout(object sender, IHostService.ClientConnectionEventArgs e)
        {
            IsStudentConnected = false;

            // Student left... The question won't ever be answered now...
            // Clean up in preperation for next student.
            MathQuestionRepositoryService.ClearActiveQuestion();

            // Clear input fields
            ClearFields();
        }

        private void HostService_DataReceived(object sender, IHostService.DataReceivedEventArgs e)
        {
            var packetType = SPacketBase.ValidateHeader(e.Data);

            // Create packet instance and deserialize.
            var constructor = packetType.GetConstructor(new Type[0]);
            var packet = (SPacketBase)constructor.Invoke(null);
            packet.Deserialize(e.Data);

            // Call appropriate received callback.
            if (packetType == typeof(AnswerPacket))
                OnReceivedAnswerPacket((AnswerPacket)packet);
        }

        #endregion Source Group - HostService

        private void MathQuestionRepositoryService_ActiveQuestionChanged(object sender, MathQuestion activeQuestion)
        {
            IsPendingAnswer = (activeQuestion != null);
        }

        /// <summary>
        /// Invoked when an <see cref="AnswerPacket"/> was received from the connected student.
        /// </summary>
        /// <param name="packet">The received packet.</param>
        private void OnReceivedAnswerPacket(AnswerPacket packet)
        {
            // Send Result //
            bool wasCorrect = (MathQuestionRepositoryService.ActiveQuestion.Answer == packet.Answer);
            SendResult(wasCorrect);

            // Answer Active Question //
            MathQuestionRepositoryService.AnswerQuestion(wasCorrect);

            // Reset State //
            ClearFields();
        }

        #endregion Callbacks
    }
}
