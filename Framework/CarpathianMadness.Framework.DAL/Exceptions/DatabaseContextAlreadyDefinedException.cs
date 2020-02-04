
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class DatabaseContextAlreadyDefinedException : Exception
    {
        #region Constructors

        internal DatabaseContextAlreadyDefinedException() : base()
        {

        }

        internal DatabaseContextAlreadyDefinedException(string message) : base(message)
        {

        }

        #endregion Constructors
    }
}
