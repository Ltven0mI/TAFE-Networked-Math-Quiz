using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClasses
{
    /**********************************************************/
    // Filename:   MathQuestion.cs
    // Purpose:    A class to represent a math question.
    //             - e.g. "10 * 4 = 40".
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-10-30
    // Tests:      N/A
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // 
    // [0.1.0] 2020-10-30
    // | [Added]
    // | - Initial Class Implementation.
    /**********************************************************/

    /// <summary>
    /// A class to represent a math question.<br/>
    /// e.g. "10 * 4 = 40".
    /// </summary>
    public class MathQuestion : IComparable<MathQuestion>
    {
        public enum MathOperator { Plus, Minus, Times, Divide };

        public double LeftOperand { get; private set; }
        public double RightOperand { get; private set; }
        public MathOperator Operator { get; private set; }
        public double Answer { get; private set; }

        public MathQuestion(double leftOperand, double rightOperand, MathOperator mathOperator)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Operator = mathOperator;
            Answer = CalculateAnswer();
        }

        /**********************************************************/
        // Method:  private double CalculateAnswer()
        // Purpose: Calculates the answer to this MathQuestion instance.
        // Returns: The calculated answer.
        // Outputs: double answer
        // Throws:  NotImplementedException - when an unknown MathOperator
        //          - is used.
        /**********************************************************/
        /// <summary>
        /// Calculates the answer to this <see cref="MathQuestion"/> instance.
        /// </summary>
        /// <returns>The calculated answer.</returns>
        /// <exception cref="NotImplementedException">when an unknown <see cref="MathOperator"/> is used.</exception>
        private double CalculateAnswer()
        {
            switch (Operator)
            {
                case MathOperator.Plus:
                    return LeftOperand + RightOperand;
                case MathOperator.Minus:
                    return LeftOperand - RightOperand;
                case MathOperator.Times:
                    return LeftOperand * RightOperand;
                case MathOperator.Divide:
                    if (RightOperand == 0) return 0; // Cannot divide by 0
                    return LeftOperand / RightOperand;
                default:
                    throw new NotImplementedException($"Unhandled value for {nameof(Operator)}: '{Operator}'");
            }
        }

        /**********************************************************/
        // Method:  public int CompareTo(MathQuestion other)
        // Purpose: Compares this instance and 'other' numerically
        //          - by their 'Answer' properties.
        // Returns: '0' if both instances' 'Answer' properties are equal.
        // Returns: '-1' if this instance's 'Answer' is less than
        //          - the 'Answer' property of 'other'.
        // Returns: '1' if this instance's 'Answer' is greater than
        //          - the 'Answer' property of 'other'.
        // Inputs:  MathQuestion other
        // Outputs: int result
        /**********************************************************/
        /// <summary>
        /// Compares this instance and <paramref name="other"/> numerically
        /// by their <c>Answer</c> properties.
        /// </summary>
        /// <param name="other">The <see cref="MathQuestion"/> to compare against.</param>
        /// <returns>
        /// <c>0</c> if both instances' <c>Answer</c> properties are equal.<br/>
        /// <c>-1</c> if this instance's <c>Answer</c> is less than
        /// the <c>Answer</c> property of <paramref name="other"/>.<br/>
        /// <c>1</c> if this instance's <c>Answer</c> is greater than
        /// the <c>Answer</c> property of <paramref name="other"/>.
        /// </returns>
        public int CompareTo(MathQuestion other)
        {
            return Answer.CompareTo(other.Answer);
        }

        /**********************************************************/
        // Method:  public override string ToString()
        // Purpose: Gets the string representation of this instance.
        //          - e.g. "40(10 * 4)".
        // Returns: The string representation.
        // Outputs: string str
        /**********************************************************/
        /// <summary>
        /// Gets the string representation of this instance.<br/>
        /// e.g. "40(10 * 4)".
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return $"{Answer}({LeftOperand} {Operator} {RightOperand})";
        }
    }
}
