
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Entity abstract class which if implemented will
    /// treat the inherited class as a read-only record, suitable
    /// for representing data from Views or read-only tables.
    /// </summary>
    [DataContract]
    public abstract class Entity<TType> where TType : Entity<TType>, new()
    {
        #region Internal Methods

        internal virtual void InternalLoad(DbDataReader reader)
        {
            OnLoading(reader);
        }

        #endregion Internal Methods

        #region Public Methods

        public static TType LoadFromDataRow(DataRow row)
        {
            return EntityFactory<TType>.CreateFromDataRow(row);
        }

        public static TType LoadFromDataReader(DbDataReader reader)
        {
            return EntityFactory<TType>.CreateFromReader(reader);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract void OnLoading(DbDataReader reader);

        #endregion Protected Methods
    }
}
