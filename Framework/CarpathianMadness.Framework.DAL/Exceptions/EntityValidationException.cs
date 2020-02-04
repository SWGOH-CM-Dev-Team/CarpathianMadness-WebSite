
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class EntityValidationException : Exception
    {
        #region Constructors

        public EntityValidationException() : base()
        {

        }

        public EntityValidationException(string message) : base(message)
        {

        }

        #endregion Constructors
    }
}