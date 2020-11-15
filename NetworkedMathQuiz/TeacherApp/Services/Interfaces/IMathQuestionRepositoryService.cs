using CommonClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TeacherApp.Services.Interfaces
{
    public interface IMathQuestionRepositoryService
    {
        /// <summary>
        /// Invoked when the value of <see cref="ActiveQuestion"/> is changed.
        /// </summary>
        public event EventHandler<MathQuestion> ActiveQuestionChanged;

        /// <summary>
        /// Invoked when the <see cref="ActiveQuestion"/> has been answered.
        /// </summary>
        /// <see cref="AnswerQuestion(bool)"/>
        public event EventHandler<bool> ActiveQuestionAnswered;

        /// <summary>
        /// The most recent <see cref="MathQuestion"/> to be asked.
        /// </summary>
        /// <seealso cref="AskQuestion(MathQuestion)"/>
        public MathQuestion ActiveQuestion { get; }

        /// <summary>
        /// A collection of all <see cref="MathQuestion"/>(s) that have been asked.
        /// </summary>
        /// <seealso cref="AskQuestion(MathQuestion)"/>
        public ReadOnlyObservableCollection<MathQuestion> AskedQuestions { get; }

        /// <summary>
        /// A collection of all incorrectly answered <see cref="MathQuestion"/>(s).<br/>
        /// <b>NOTE:</b> This property returns a clone of the internal <see cref="LinkedList{T}"/>,
        /// so modifications to the returned <see cref="LinkedList{T}"/> won't effect the internal <see cref="LinkedList{T}"/>.
        /// </summary>
        public LinkedList<MathQuestion> IncorrectQuestions { get; }

        /// <summary>
        /// Set <see cref="ActiveQuestion"/> to <paramref name="mathQuestion"/> and add it to the <see cref="AskedQuestions"/> collection.
        /// </summary>
        /// <param name="mathQuestion">The <see cref="MathQuestion"/> that was asked.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="mathQuestion"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        /// when either there's an existing <see cref="ActiveQuestion"/>, or <paramref name="mathQuestion"/> has already been asked.
        /// </exception>
        public void AskQuestion(MathQuestion mathQuestion);

        /// <summary>
        /// Provide an answer for the current <see cref="ActiveQuestion"/>.<br/>
        /// Clears the <see cref="ActiveQuestion"/> and invokes <see cref="ActiveQuestionAnswered"/>.<br/>
        /// <b>NOTE:</b> if <paramref name="wasCorrect"/> is <c>false</c>, the <see cref="ActiveQuestion"/>
        /// will also be added to the <see cref="IncorrectlyAnswered"/> collection.
        /// </summary>
        /// <param name="wasCorrect">Whether the question was answered correctly or not.</param>
        /// <exception cref="InvalidOperationException">when <see cref="ActiveQuestion"/> is null.</exception>
        public void AnswerQuestion(bool wasCorrect);

        /// <summary>
        /// Clears the current <see cref="ActiveQuestion"/>.
        /// </summary>
        public void ClearActiveQuestion();

        /// <summary>
        /// Checks whether or not a <see cref="MathQuestion"/> matching <paramref name="mathQuestion"/> has already been asked.
        /// </summary>
        /// <param name="mathQuestion">The question to check for.</param>
        /// <returns><c>true</c> if <paramref name="mathQuestion"/> has already been asked, otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">when <paramref name="mathQuestion"/> is <c>null</c>.</exception>
        public bool DoesQuestionExist(MathQuestion mathQuestion);

        /// <summary>
        /// Search for an asked <see cref="MathQuestion"/> using it's string representation.<br/>
        /// e.g. <c>"4 * 10 = 40"</c>.
        /// </summary>
        /// <param name="searchQuery">The <see cref="MathQuestion"/> string to search for.</param>
        /// <returns>
        /// <see cref="MathQuestion"/> if found, otherwise <c>null</c>.
        /// </returns>
        public MathQuestion SearchQuestion(string searchQuery);

        /// <summary>
        /// Sort <see cref="AskedQuestions"/> using the passed <paramref name="method"/> and <paramref name="direction"/>.
        /// </summary>
        /// <param name="method">The method of sorting.</param>
        /// <param name="direction">The direction of sorting.</param>
        public void SortAskedQuestions(SortingMethod method, SortingDirection direction);

        /// <summary>
        /// Get a <see cref="IReadOnlyList{T}"/> containing all asked questions, from an internal <see cref="BinaryTree{T}"/>
        /// traversed using the specified <paramref name="order"/>.
        /// </summary>
        /// <param name="order">The desired <see cref="TraversalOrder"/> to use.</param>
        /// <returns>An <see cref="IReadOnlyList{T}"/> of all asked questions traversed.</returns>
        /// <exception cref="NotSupportedException">when an unsupported <see cref="TraversalOrder"/> is passed in <paramref name="order"/>.</exception>
        public IReadOnlyList<MathQuestion> TraverseBinaryTreeQuestions(TraversalOrder order);
    }
}
