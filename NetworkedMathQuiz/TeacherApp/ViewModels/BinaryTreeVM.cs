using BinaryTree;
using CommonClasses;
using CommonClasses.Utilities;
using MVVMUtil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TeacherApp.Services.Interfaces;
using TeacherApp.ViewModels.Interfaces;

namespace TeacherApp.ViewModels
{
    public class BinaryTreeVM : ViewModelBase, IViewBinaryTreeVM
    {
        #region Properties

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

        #endregion Properties


        #region Commands

        #region Command - SearchCommand
        private RelayCommand _searchCommand;
        public ICommand SearchCommand
        {
            get => _searchCommand ??=
                new RelayCommand(
                    _ =>
                    {
                        SearchAskedQuestions(SearchText);
                    }
                );
        }
        #endregion Command - SearchCommand

        #region Command - OrderDisplayCommand
        private RelayCommand _orderDisplayCommand;
        public ICommand OrderDisplayCommand
        {
            get => _orderDisplayCommand ??=
                new RelayCommand(
                    order =>
                    {
                        DisplayBinaryTree((TraversalOrder)order);
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
                        SaveBinaryTree((TraversalOrder)order);
                    }
                );
        }
        #endregion Command - OrderSaveCommand

        #endregion Commands


        #region Services

        #region Service - MathQuestionRepositoryService
        [Unity.Dependency]
        public IMathQuestionRepositoryService MathQuestionRepositoryService { get; set; }
        #endregion Service - MathQuestionRepositoryService

        #region Service - FileWriter
        [Unity.Dependency]
        public IFileWriterService FileWriter { get; set; }
        #endregion Service - FileWriter

        #endregion Services


        #region Models
        #endregion Models


        #region ViewModels
        #endregion ViewModels


        // Constructors //
        public BinaryTreeVM() { }


        /// <summary>
        /// Search for a <see cref="MathQuestion"/> matching the <paramref name="searchQuery"/>
        /// and display it in the <see cref="BinaryTreeText"/> property.
        /// </summary>
        /// <param name="searchQuery">The <see cref="MathQuestion"/> string to search for.</param>
        private void SearchAskedQuestions(string searchQuery)
        {
            // IF: No Questions Asked Yet //
            if (MathQuestionRepositoryService.AskedQuestions.Count == 0)
            {
                BinaryTreeText = "No math questions answered!";
                return;
            }

            // IF: Empty SearchQuery //
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                BinaryTreeText = "ERROR: Search field is empty - require math question to search in format 3 + 4 = 7";
                return;
            }

            // IF: Invalid Formatted SearchQuery //
            if (!Regex.IsMatch(searchQuery, "^(?<first>\\d*\\.?\\d*)\\s(?<operator>[+\\-*\\/])\\s(?<second>\\d*\\.?\\d*)\\s=\\s(?<answer>\\d*\\.?\\d*)$"))
            {
                BinaryTreeText = "ERROR: Math question to search not in correct format - must be similar to: 3 * 4 = 12";
                return;
            }

            // Search for Question //
            var question = MathQuestionRepositoryService.SearchQuestion(searchQuery);

            // IF: Question Not Found //
            if (question == null)
            {
                BinaryTreeText = $"'{searchQuery}' NOT found!";
                return;
            }

            // Question Found //
            BinaryTreeText = $"{question} found!";
        }

        /// <summary>
        /// Display all asked <see cref="MathQuestion"/>(s) traversed using the <paramref name="order"/>,
        /// in the <see cref="BinaryTreeText"/> property.
        /// </summary>
        /// <param name="order">The <see cref="TraversalOrder"/> to use.</param>
        private void DisplayBinaryTree(TraversalOrder order)
        {
            // IF: No Questions Asked //
            if (MathQuestionRepositoryService.AskedQuestions.Count == 0)
            {
                BinaryTreeText = "No math questions have been set up";
                return;
            }

            // Traverse Asked Questions //
            var questions = MathQuestionRepositoryService.TraverseBinaryTreeQuestions(order);

            // Generate and Display Traversal String //
            string traversalString = string.Join(", ", questions);
            BinaryTreeText = $"{order.GetDescription()}: {traversalString}";
        }

        /// <summary>
        /// Save all asked <see cref="MathQuestion"/>(s) traversed using the <paramref name="order"/>,
        /// in a text file named after the specified <see cref="TraversalOrder"/><br/>
        /// e.g. <c>"IN-ORDER.txt"</c>.
        /// </summary>
        /// <param name="order">The <see cref="TraversalOrder"/> to use.</param>
        private void SaveBinaryTree(TraversalOrder order)
        {
            // IF: No Questions Asked //
            if (MathQuestionRepositoryService.AskedQuestions.Count == 0)
            {
                BinaryTreeText = "No math questions have been set up";
                return;
            }

            // Traverse Asked Questions //
            var questions = MathQuestionRepositoryService.TraverseBinaryTreeQuestions(order);

            // Generate and Save Traversal String //
            string traversalString = string.Join(", ", questions);
            FileWriter.WriteToFile($"{order.GetDescription()}.txt", $"{order.GetDescription()}: {traversalString}");

            // Display Message to User //
            BinaryTreeText = $"Successfully saved to '{order.GetDescription()}.txt'";
        }


        #region Callbacks
        #endregion Callbacks
    }
}
