using BinaryTree;
using CommonClasses;
using MVVMUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TeacherApp.Models
{
    public class QuestionsModel : INotifyPropertyChanged
    {
        #region Properties

        #region Property - ActiveQuestion
        private MathQuestion _activeQuestion;
        public MathQuestion ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged(nameof(ActiveQuestion));
            }
        }
        #endregion Property - ActiveQuestion

        #region Property - AskedQuestions
        private ObservableCollection<MathQuestion> _askedQuestions;
        public ObservableCollection<MathQuestion> AskedQuestions
        {
            get => _askedQuestions ??=
                new ObservableCollection<MathQuestion>();
        }
        #endregion Property - AskedQuestions

        #region Property - QuestionsBinaryTree
        private BinaryTree<MathQuestion> _questionsBinaryTree;
        public BinaryTree<MathQuestion> QuestionsBinaryTree
        {
            get => _questionsBinaryTree ??=
                new BinaryTree<MathQuestion>();
        }
        #endregion Property - QuestionsBinaryTree

        #region Property - QuestionsDictionary
        private Dictionary<string, MathQuestion> _questionsDictionary;
        public Dictionary<string, MathQuestion> QuestionsDictionary
        {
            get => _questionsDictionary ??=
                new Dictionary<string, MathQuestion>();
        }
        #endregion Property - QuestionsDictionary

        #endregion Properties


        public void AddQuestion(MathQuestion mathQuestion)
        {
            AskedQuestions.Add(mathQuestion);
            QuestionsBinaryTree.Add(mathQuestion);
            QuestionsDictionary.Add(mathQuestion.ToString(), mathQuestion);
        }


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
    }
}
