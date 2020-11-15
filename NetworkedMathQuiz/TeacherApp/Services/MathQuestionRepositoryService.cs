using BinaryTree;
using CommonClasses;
using CommonClasses.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TeacherApp.Services.Interfaces;

namespace TeacherApp.Services
{
    public class MathQuestionRepositoryService : IMathQuestionRepositoryService
    {
        public event EventHandler<MathQuestion> ActiveQuestionChanged;
        public event EventHandler<bool> ActiveQuestionAnswered;


        private MathQuestion _activeQuestion;
        public MathQuestion ActiveQuestion
        {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                ActiveQuestionChanged?.Invoke(this, _activeQuestion);
            }
        }

        private Dictionary<string, MathQuestion> _questionDictionary;
        private BinaryTree<MathQuestion> _questionBinaryTree;

        private LinkedList<MathQuestion> _incorrectQuestions;
        public LinkedList<MathQuestion> IncorrectQuestions { get => new LinkedList<MathQuestion>(_incorrectQuestions); }

        private ObservableCollection<MathQuestion> _askedQuestions;
        public ReadOnlyObservableCollection<MathQuestion> AskedQuestions { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MathQuestionRepositoryService"/> class.
        /// </summary>
        public MathQuestionRepositoryService()
        {
            // Variable Initialization //
            _questionDictionary = new Dictionary<string, MathQuestion>();
            _questionBinaryTree = new BinaryTree<MathQuestion>();

            _incorrectQuestions = new LinkedList<MathQuestion>();

            _askedQuestions = new ObservableCollection<MathQuestion>();
            AskedQuestions = new ReadOnlyObservableCollection<MathQuestion>(_askedQuestions);
        }

        public void AskQuestion(MathQuestion mathQuestion)
        {
            // Argument Validation //
            if (mathQuestion == null)
                throw new ArgumentNullException(nameof(mathQuestion));

            // State Validation //
            if (ActiveQuestion != null)
                throw new InvalidOperationException("Cannot ask question, there is an existing active question.");
            if (DoesQuestionExist(mathQuestion))
                throw new InvalidOperationException("Cannot ask question, this question has already been asked.");

            // Store Question //
            _questionDictionary.Add(mathQuestion.ToString(), mathQuestion);
            _questionBinaryTree.Add(mathQuestion);
            _askedQuestions.Add(mathQuestion);
            ActiveQuestion = mathQuestion;
        }

        public void AnswerQuestion(bool wasCorrect)
        {
            // State Validation //
            if (ActiveQuestion == null)
                throw new InvalidOperationException("Cannot answer question, there is no active question.");

            if (!wasCorrect)
                _incorrectQuestions.AddLast(ActiveQuestion);

            // Answer Question //
            ActiveQuestion = null;
            ActiveQuestionAnswered?.Invoke(this, wasCorrect);
        }

        public void ClearActiveQuestion()
        {
            ActiveQuestion = null;
        }

        public bool DoesQuestionExist(MathQuestion mathQuestion)
        {
            // Argument Validation //
            if (mathQuestion == null)
                throw new ArgumentNullException(nameof(mathQuestion));

            return _questionDictionary.ContainsKey(mathQuestion.ToString());
        }

        public MathQuestion SearchQuestion(string searchQuery)
        {
            return _questionDictionary.GetValueOrDefault(searchQuery, null);
        }

        public void SortAskedQuestions(SortingMethod method, SortingDirection direction)
        {
            // IF: AskedQuestions is empty //
            if (_askedQuestions.Count == 0)
                return;

            // Construct Array //
            var array = new MathQuestion[_askedQuestions.Count];
            _askedQuestions.CopyTo(array, 0);

            // Sort Array //
            SortingUtil.SortArray(array, method, direction);

            // Replace Array //
            for (int i = 0; i < array.Length; i++)
                _askedQuestions[i] = array[i];
        }

        public IReadOnlyList<MathQuestion> TraverseBinaryTreeQuestions(TraversalOrder order)
        {
            // Set Traversal Strategy //
            _questionBinaryTree.TraversalStrategy = order switch
            {
                TraversalOrder.InOrder => new InOrderTraversal<MathQuestion>(),
                TraversalOrder.PostOrder => new PostOrderTraversal<MathQuestion>(),
                TraversalOrder.PreOrder => new PreOrderTraversal<MathQuestion>(),
                _ => throw new NotSupportedException($"The {nameof(TraversalOrder)} '{order}' is not currently supported.")
            };

            // Return a List from the Binary Tree //
            return new List<MathQuestion>(_questionBinaryTree);
        }
    }
}
