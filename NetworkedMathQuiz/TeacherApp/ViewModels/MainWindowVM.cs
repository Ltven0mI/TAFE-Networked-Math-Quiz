using BinaryTree;
using CommonClasses;
using CommonClasses.Utilities;
using MVVMUtil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TeacherApp.Models;
using TeacherApp.ViewModels.Interfaces;

namespace TeacherApp.ViewModels
{
    public class MainWindowVM : ViewModelBase, IViewMainWindowVM
    {
        #region Properties

        #region Property - FirstNumber
        private double _firstNumber;
        public double FirstNumber
        {
            get => _firstNumber;
            set
            {
                _firstNumber = Math.Round(value, MathQuestion.NUMBER_SCALE);
                RaisePropertyChanged(nameof(FirstNumber));
            }
        }
        #endregion Property - FirstNumber

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
        private double _secondNumber;
        public double SecondNumber
        {
            get => _secondNumber;
            set
            {
                _secondNumber = Math.Round(value, MathQuestion.NUMBER_SCALE);
                RaisePropertyChanged(nameof(SecondNumber));
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

        #region Property - BinaryTreeText
        private string _binaryTreeText;
        public string BinaryTreeText
        {
            get => _binaryTreeText;
            private set
            {
                _binaryTreeText = value;
                RaisePropertyChanged(nameof(BinaryTreeText));
            }
        }
        #endregion Property - BinaryTreeText

        #region Property - SearchText
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisePropertyChanged(nameof(SearchText));
            }
        }
        #endregion Property - SearchText

        #region Property - IsPendingAnswer
        private bool _isPendingAnswer;
        public bool IsPendingAnswer
        {
            get => _isPendingAnswer;
            private set
            {
                _isPendingAnswer = value;
                RaisePropertyChanged(nameof(IsPendingAnswer));
            }
        }
        #endregion Property - IsPendingAnswer

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
                        SendQuestion(new MathQuestion(FirstNumber, SecondNumber, (MathOperator)SelectedOperatorIndex));
                    },
                    _ =>
                    {
                        return !IsPendingAnswer;
                    }
                );
        }
        #endregion Command - SendCommand

        #region Command - DisplayLinkedListCommand
        private RelayCommand _displayLinkedListCommand;
        public ICommand DisplayLinkedListCommand
        {
            get => _displayLinkedListCommand ??=
                new RelayCommand(
                    _ =>
                    {
                        MessageBox.Show("Display Linked List Command Pressed");
                    },
                    _ =>
                    {
                        return true;
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
                        ((IClosable)closable).Close();
                    },
                    _ =>
                    {
                        return true;
                    }
                );
        }
        #endregion Command - ExitCommand

        #region Command - SearchCommand
        private RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get => _searchCommand ??=
                new RelayCommand(
                    _ =>
                    {
                        MessageBox.Show("Search Command Pressed");
                    },
                    _ =>
                    {
                        return true;
                    }
                );
        }
        #endregion Command - SearchCommand

        #region Command - SortCommand
        private RelayCommand _sortCommand;
        public ICommand SortCommand
        {
            get => _sortCommand ??=
                new RelayCommand(
                    sortType =>
                    {
                        MessageBox.Show($"{sortType} Sort Command Pressed");
                    },
                    _ =>
                    {
                        return true;
                    }
                );
        }
        #endregion Command - SortCommand

        #region Command - OrderDisplayCommand
        private RelayCommand _orderDisplayCommand;
        public ICommand OrderDisplayCommand
        {
            get => _orderDisplayCommand ??=
                new RelayCommand(
                    order =>
                    {
                        // No Questions Asked //
                        if (QuestionsModel.QuestionsBinaryTree.Count == 0)
                        {
                            BinaryTreeText = "No math questions have been set up";
                            return;
                        }

                        // Determine Traversal Strategy and Order String.
                        var orderType = (OrderTypes)order;
                        ITraversalStrategy<MathQuestion> traversalStrategy = null;
                        switch(orderType)
                        {
                            case OrderTypes.PreOrder:
                                traversalStrategy = new PreOrderTraversal<MathQuestion>();
                                break;
                            case OrderTypes.InOrder:
                                traversalStrategy = new InOrderTraversal<MathQuestion>();
                                break;
                            case OrderTypes.PostOrder:
                                traversalStrategy = new PostOrderTraversal<MathQuestion>();
                                break;
                            default:
                                throw new NotImplementedException($"Unsupported OrderType: '{order}'.");
                        }

                        // Update Traversal Strategy.
                        QuestionsModel.QuestionsBinaryTree.TraversalStrategy = traversalStrategy;

                        // Generate Traversal String and Display.
                        string traversalString = string.Join(", ", QuestionsModel.QuestionsBinaryTree);
                        BinaryTreeText = $"{orderType.GetDescription()}: {traversalString}";
                    },
                    _ =>
                    {
                        return true;
                    }
                );
        }
        #endregion Command - OrderDisplayCommand

        #region Command - OrderSaveCommand
        private RelayCommand _orderSaveCommand;
        public ICommand OrderSaveCommand
        {
            get => _orderSaveCommand ??=
                new RelayCommand(
                    order =>
                    {
                        var orderType = (OrderTypes)order;

                        // No Questions Asked //
                        if (QuestionsModel.QuestionsBinaryTree.Count == 0)
                        {
                            BinaryTreeText = $"There are no math questions to save as {orderType.GetDescription()}!";
                            return;
                        }

                        MessageBox.Show("Not implemented yet!");
                    },
                    _ =>
                    {
                        return true;
                    }
                );
        }
        #endregion Command - OrderSaveCommand


        #endregion Commands

        #region Models

        #region Model - QuestionsModel
        private QuestionsModel _questionsModel;
        public QuestionsModel QuestionsModel
        {
            get => _questionsModel ??=
                new QuestionsModel();
        }
        #endregion Model - QuestionsModel

        #endregion Models

        #region ViewModels
        #endregion ViewModels

        #region Services
        #endregion Services

        public MainWindowVM()
        {

        }

        private void SendQuestion(MathQuestion question)
        {
            // State Validation //
            if (QuestionsModel.ActiveQuestion != null)
                throw new InvalidOperationException("Cannot send a question while there is already a pending question.");

            AnswerString = $"{question.Answer}";
            QuestionsModel.ActiveQuestion = question;
            QuestionsModel.AddQuestion(question);
            IsPendingAnswer = true;
        }

        #region Callback Methods
        #endregion Callback Methods
    }
}
