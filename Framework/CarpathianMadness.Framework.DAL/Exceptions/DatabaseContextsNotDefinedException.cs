
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class DatabaseContextsNotDefinedException : Exception
    {
        #region Constructors

        internal DatabaseContextsNotDefinedException() : base()
        {

        }

        internal DatabaseContextsNotDefinedException(string message) : base(message)
        {

        }

        #endregion Constructors
    }
}
