
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpathianMadness.Framework
{
    public sealed class ArgumentStringException : ArgumentException
    {
        #region Constructors

        public ArgumentStringException() : base()
        {

        }

        public ArgumentStringException(string param)
            : base("Cannot be null, empty or whitespace.", param)
        {

        }

        #endregion Constructors
    }
}