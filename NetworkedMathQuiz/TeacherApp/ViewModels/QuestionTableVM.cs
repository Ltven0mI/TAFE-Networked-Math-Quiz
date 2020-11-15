using CommonClasses;
using CommonClasses.Utilities;
using MVVMUtil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TeacherApp.Services.Interfaces;
using TeacherApp.ViewModels.Interfaces;

namespace TeacherApp.ViewModels
{
    public class QuestionTableVM : ViewModelBase, IViewQuestionTableVM
    {
        #region Properties
        #endregion Properties


        #region Commands

        #region Command - SortCommand
        private RelayCommand _sortCommand;
        public ICommand SortCommand
        {
            get => _sortCommand ??=
                new RelayCommand(
                    arg =>
                    {
                        // Extract Args //
                        var args = (object[])arg;
                        var sortingMethod = (SortingMethod)args[0];
                        var sortingDirection = (SortingDirection)args[1];

                        // IF: No Questions Asked //
                        if (MathQuestionRepositoryService.AskedQuestions.Count == 0)
                        {
                            var msg = $"Unable to run a {sortingMethod.GetDescription()} sort.{Environment.NewLine}" +
                                "No math questions have been sent!";
                            MessageBox.Show(msg, "ERROR");
                            return;
                        }
                        
                        // Sort Asked Questions //
                        MathQuestionRepositoryService.SortAskedQuestions(sortingMethod, sortingDirection);
                    }
                );
        }
        #endregion Command - SortCommand

        #endregion Commands


        #region Services

        #region Service - MathQuestionRepositoryService
        [Unity.Dependency]
        public IMathQuestionRepositoryService MathQuestionRepositoryService { get; set; }
        #endregion Service - MathQuestionRepositoryService

        #endregion Services


        // Constructors //
        public QuestionTableVM() { }


        #region Callbacks
        #endregion Callbacks
    }
}
