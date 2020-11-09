using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMUtil
{
    /**********************************************************/
    // Filename:   UnknownPropertyException.cs
    // Purpose:    Represents an error that occurs during validation
    //             - of a property name, if the property doesn't exist.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-09-24
    // Tests:      N/A
    /**********************************************************/

    /// <summary>
    /// Represents an error that occurs during validation of a property name,
    /// if the property doesn't exist.
    /// </summary>
    public class UnknownPropertyException : Exception
    {
        /// <summary>
        /// The name of the property that doesn't exist.
        /// </summary>
        public string PropertyName { get; }


        /**********************************************************/
        // Method:  public UnknownPropertyException(string message)
        // Purpose: Initializes a new instance of the UnknownPropertyException
        //          - class with a specified error message.
        // Inputs:  string message
        /**********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownPropertyException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public UnknownPropertyException(string message) : this(string.Empty, message) { }


        /**********************************************************/
        // Method:  public UnknownPropertyException(string propertyName, string message)
        // Purpose: Initializes a new instance of the UnknownPropertyException
        //          - class with a specified property name and error message.
        // Inputs:  string propertyName, string message
        /**********************************************************/

        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownPropertyException"/> class
        /// with a specified property name and error message.
        /// </summary>
        /// <param name="propertyName">The name of the property that doesn't exist.</param>
        /// <param name="message">The error message.</param>
        public UnknownPropertyException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
