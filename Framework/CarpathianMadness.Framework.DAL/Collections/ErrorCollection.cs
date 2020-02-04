
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    [CollectionDataContract]
    public sealed class ErrorCollection : List<Error>
    {
        #region Public Methods

        public void AddValue(string message)
        {
            AddValue(0, message);
        }

        public void AddValue(long code, string message)
        {
            this.Add(new Error(code, message));
        }

        #endregion Public Methods
    }
}
