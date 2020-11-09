using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MVVMUtil
{
    /**********************************************************/
    // Filename:   RelayCommand.cs
    // Purpose:    Provides a concrete implementation of the
    //             - ICommand interface,
    //             - utilizing Action<T>(s) and Predicate<T>(s).
    //             - NOTE: Original implementation from:
    //             - https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern#relaying-command-logic
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-09-24
    // Tests:      N/A
    /**********************************************************/

    /// <summary>
    /// Provides a concrete implementation of the <see cref="ICommand"/> interface,
    /// utilizing <see cref="Action{T}"/>(s) and <see cref="Predicate{T}"/>(s).
    /// <para/>
    /// <b>NOTE</b>: Original implementation from:<br/>
    /// https://docs.microsoft.com/en-us/archive/msdn-magazine/2009/february/patterns-wpf-apps-with-the-model-view-viewmodel-design-pattern#relaying-command-logic
    /// </summary>
    public class RelayCommand : ICommand
    {
        protected readonly Action<object> _execute;
        protected readonly Predicate<object> _canExecute;


        /**********************************************************/
        // Method:  public RelayCommand(Action<object> execute)
        // Purpose: Constructs a new RelayCommand with the specified
        //          - 'execute' and the default 'CanExecute',
        //          - i.e. RelayCommand can always execute.
        // Inputs:  Action<object> execute
        // Throws:  ArgumentNullException - when 'execute' is null.
        /**********************************************************/

        /// <summary>
        /// Constructs a new <see cref="RelayCommand"/> with the specified
        /// <paramref name="execute"/> and the default <c>CanExecute</c>,
        /// i.e. <see cref="RelayCommand"/> can always execute.
        /// </summary>
        /// <param name="execute">
        /// The <see cref="Action{T}"/> to perform when <see cref="Execute(object)"/> is called.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="execute"/> is <c>null</c>.</exception>
        public RelayCommand(Action<object> execute) : this(execute, null) { }


        /**********************************************************/
        // Method:  public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        // Purpose: Constructs a new RelayCommand with the specified
        //          - 'execute' and 'canExecute'.
        // Inputs:  Action<object> execute, Predicate<object> canExecute
        // Throws:  ArgumentNullException - when 'execute' is null.
        /**********************************************************/

        /// <summary>
        /// Constructs a new <see cref="RelayCommand"/> with the specified
        /// <paramref name="execute"/> and <paramref name="canExecute"/>.
        /// </summary>
        /// <param name="execute">
        /// The <see cref="Action{T}"/> to perform when <see cref="Execute(object)"/> is called.
        /// </param>
        /// <param name="canExecute">
        /// The <see cref="Predicate{object}"/> used when <see cref="CanExecute(object)"/> is called.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="execute"/> is <c>null</c>.</exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }


        /**********************************************************/
        // Method:  public bool CanExecute(object param)
        // Purpose: Gets whether this RelayCommand can execute at this time.
        // Returns: true or false depending on whether it can execute or not.
        // Inputs:  object param
        // Outputs: bool canExecute
        /**********************************************************/

        /// <summary>
        /// Gets whether this <see cref="RelayCommand"/> can execute at this time.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns><c>true</c> or <c>false</c> depending on whether it can execute or not.</returns>
        public bool CanExecute(object param)
        {
            return _canExecute == null ? true : _canExecute(param);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        /**********************************************************/
        // Method:  public void Execute(object param)
        // Purpose: Executes the RelayCommand's internal Action.
        // Inputs:  object param
        /**********************************************************/

        /// <summary>
        /// Executes the <see cref="RelayCommand"/>'s internal <see cref="Action"/>.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public void Execute(object param)
        {
            _execute(param);
        }
    }
}
