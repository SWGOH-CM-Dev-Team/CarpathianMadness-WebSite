
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using CarpathianMadness.Framework;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Represents a collection of SQL Parameters.
    /// </summary>
    public sealed class CommandParameterCollection : List<CommandParameter>
    {
        #region Members

        private IReadOnlyDictionary<string, object> _outputValues;

        #endregion Members

        #region Constants

        private static readonly IReadOnlyDictionary<string, object> _Empty = new Dictionary<string, object>(0);

        #endregion Constants

        #region Properties

        /// <summary>
        /// Get a read-only dictionary of output values, this is populated
        /// when this instance is passed to a QueryContext to be executed
        /// against a database.
        /// </summary>
        public IReadOnlyDictionary<string, object> OutputValues
        {
            get
            {
                return this._outputValues != null ? this._outputValues : _Empty;
            }

            internal set
            {
                this._outputValues = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Adds a new parameter populated using the provided values.
        /// </summary>
        public void AddValue(string parameterName, object value, DbType dbType)
        {

            this.Add(new CommandParameter(parameterName, ParseNull(value), dbType));
        }

        /// <summary>
        /// Adds a new parameter populated using the provided values.
        /// </summary>
        public void AddValue(string parameterName, DbType dbType, ParameterDirection direction)
        {
            this.Add(new CommandParameter(parameterName, DBNull.Value, dbType, direction));
        }

        /// <summary>
        /// Adds a new parameter populated using the provided values.
        /// </summary>
        public void AddValue(string parameterName, object value, DbType dbType, ParameterDirection direction)
        {
            this.Add(new CommandParameter(parameterName, ParseNull(value), dbType, direction));
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Creates an array of parameters of specific DbParameter instances based
        /// on the provided DatabaseContext.
        ///</summary>
        internal object ParseNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            {
                return value;
            }
        }

        /// <summary>
        /// Creates an array of parameters of specific DbParameter instances based
        /// on the provided DatabaseContext.
        /// </summary>
        internal DbParameter[] ToDbParameters(DatabaseContext context)
        {
            DbParameter[] parameters = new DbParameter[this.Count];

            if (this.Count > 0)
            {
                Provider provider = context.Provider;

                for (int i = 0; i < this.Count; i++)
                {
                    CommandParameter item = this[i];
                    parameters[i] = provider.CreateParameter(item);
                }
            }

            return parameters;
        }

        #endregion Internal Methods
    }
}
