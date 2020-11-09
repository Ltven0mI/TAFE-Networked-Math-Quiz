using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMUtil
{
    /**********************************************************/
    // Filename:   IClosable.cs
    // Purpose:    A wrapper interface for classes that can be closed.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-09-24
    // Tests:      N/A
    /**********************************************************/

    /// <summary>
    /// A wrapper interface for classes that can be closed.
    /// </summary>
    public interface IClosable
    {
        public void Close();
    }
}
