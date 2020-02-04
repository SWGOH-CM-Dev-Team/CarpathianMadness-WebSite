
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Generic SQL parameter struct to add parameters to CommandParameterCollection which will
    /// generate the necessary DbParameter class depending on the DatabaseContext of a query.
    /// </summary>
    public struct CommandParameter
    {
        #region Properties

        /// <summary>
        /// Get/Set the parameter name.
        /// </summary>
        public string ParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set the value of the parameter.
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set the DbType of the parameter value.
        /// </summary>
        public DbType DbType
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set the direction of the parameter.
        /// </summary>
        public ParameterDirection Direction
        {
            get;
            set;
        }

        #endregion Properties

        #region Constructors

        public CommandParameter(string parameterName, object value, DbType dbType) : this(parameterName, value, dbType, ParameterDirection.Input)
        {

        }

        public CommandParameter(string parameterName, object value, DbType dbType, ParameterDirection direction) : this()
        {
            ParameterName = parameterName;
            Value = value;
            DbType = dbType;
            Direction = direction;
        }

        #endregion Constructors
    }
}