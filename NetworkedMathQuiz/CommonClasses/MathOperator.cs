using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommonClasses
{
    public enum MathOperator
    {
        [Description("+")]
        Plus,
        [Description("-")]
        Minus,
        [Description("/")]
        Divide,
        [Description("*")]
        Times
    }
}
