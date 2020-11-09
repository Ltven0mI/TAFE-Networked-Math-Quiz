using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Networking
{
    /**********************************************************/
    // Filename:   InvalidHeaderException.cs
    // Purpose:    Represents errors that occur during deserialization
    //             - of SPackets.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-11-01
    // Tests:      N/A
    /**********************************************************/

    /// <summary>
    /// Represents errors that occur during deserialization of SPackets.
    /// </summary>
    public class InvalidHeaderException : Exception
    {
        /**********************************************************/
        // Method:  public InvalidHeaderException()
        // Purpose: Initializes a new instance of the
        //          - InvalidHeaderException class.
        /**********************************************************/
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class.
        /// </summary>
        public InvalidHeaderException() : base() { }

        /**********************************************************/
        // Method: public InvalidHeaderException(string message)
        // Purpose: Initializes a new instance of the
        //          - InvalidHeaderException class with a specified
        //          - error message.
        // Inputs:  string message
        /**********************************************************/
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidHeaderException(string message) : base(message) { }

        /**********************************************************/
        // Method:  public InvalidHeaderException(string message, Exception inner)
        // Purpose: Initializes a new instance of the
        //          - InvalidHeaderException class with a specified
        //          - error message and a reference to the inner
        //          - exception that is the cause of this exception.
        // Inputs:  string message, Exception inner
        /**********************************************************/
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidHeaderException"/> class
        /// with a specified error message and a reference to the inner exception
        /// that is the cause of this exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidHeaderException(string message, Exception inner)
            : base(message, inner) { }
    }
}
