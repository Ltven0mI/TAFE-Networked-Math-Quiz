using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommonClasses
{
    public enum OrderTypes
    {
        [Description("PRE-ORDER")]
        PreOrder,

        [Description("IN-ORDER")]
        InOrder,

        [Description("POST-ORDER")]
        PostOrder
    }
}
