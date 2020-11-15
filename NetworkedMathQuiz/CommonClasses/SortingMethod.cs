using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommonClasses
{
    public enum SortingMethod
    {
        [Description("BUBBLE")]
        BubbleSort,

        [Description("SELECTION")]
        SelectionSort,

        [Description("INSERTION")]
        InsertionSort
    }
}
