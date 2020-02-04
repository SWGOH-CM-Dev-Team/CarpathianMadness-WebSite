
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Abstract class representing a provider which handles
    /// creating the various driver related implementions
    /// of DbCommand, DbConnection, etc for a database.
    /// </summary>
    public abstract class Provider
    {
        #region Members

        private readonly object _serverDateTimeLock;
        private DateTime _serverDateTime;
        private DateTime _lastObtained;

        #endregion Members

        #region Properties

        /// <summary>
        /// Get the CommandTimeout.
        /// </summary>
        public int CommandTimeout
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the ConnectionStringSettings for this provider.
        /// </summary>
        public ConnectionStringSettings ConnectionStringSettings
        {
            get;
            private set;
        }

        #endregion Properties

        #region Constructors

        internal Provider(int commandTimeout, ConnectionStringSettings connectionStringSettings)
        {
            this.CommandTimeout = commandTimeout;
            this.ConnectionStringSettings = connectionStringSettings;
            this._serverDateTimeLock = new object();
            this._serverDateTime = DateTime.MinValue;
            this._lastObtained = DateTime.MinValue;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Creates a new instance of DbConnection implemented based on the provider type.
        /// </summary>
        public abstract DbConnection CreateConnection();

        /// <summary>
        /// Creates a new instance of DbCommand implemented based on the provider type.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public virtual DbCommand CreateCommand(string commandText, CommandType commandType, DbConnection connection)
        {
            DbCommand command = this.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Connection = connection;

            return command;
        }

        /// <summary>
        /// Creates a new instance of DbDataAdapter implemented based on the provider type.
        /// </summary>
        public abstract DbDataAdapter CreateDataAdapter();

        /// <summary>
        /// Creates a new instance of DbParameter implemented based on the provider type.
        /// </summary>
        public abstract DbParameter CreateParameter();

        /// <summary>
        /// Creates a new instance of DbParameter implemented based on the provider type, using the provided values.
        /// </summary>
        public DbParameter CreateParameter(string parameterName, object value, DbType dbType, ParameterDirection direction)
        {
            DbParameter parameter = CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            parameter.DbType = dbType;
            parameter.Direction = direction;

            return parameter;
        }

        /// <summary>
        /// Creates a new instance of DbParameter implemented based on the provider type, using the provided generic commandParameter.
        /// </summary>
        public DbParameter CreateParameter(CommandParameter commandParameter)
        {
            return CreateParameter(commandParameter.ParameterName, commandParameter.Value, commandParameter.DbType, commandParameter.Direction);
        }

        /// <summary>
        /// Returns the current utc date/time from the database server.
        /// </summary>
        public DateTime GetUtcDateTime(QueryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            lock (this._serverDateTimeLock)
            {
                TimeSpan lastChecked = DateTime.UtcNow - this._lastObtained;

                if (lastChecked.TotalMinutes >= 5)
                {
                    //DateTime start = DateTime.UtcNow;
                    this._serverDateTime = context.ExecuteScalar<DateTime>(this.GetDatabaseServerDateTimeScript(), CommandType.Text);
                    //DateTime end = DateTime.UtcNow;

                    //this._serverDateTime.Add(end - start);
                    this._lastObtained = DateTime.UtcNow;

                    lastChecked = DateTime.UtcNow - this._lastObtained;
                }

                return DateTime.SpecifyKind(this._serverDateTime.Add(lastChecked), DateTimeKind.Utc);
            }
        }

        public DateTime GetLocalTime(QueryContext context)
        {
            return this.GetLocalTime(TimeZoneInfo.Local, context);
        }

        public DateTime GetLocalTime(TimeZoneInfo timeZone, QueryContext context)
        {
            if (timeZone == null)
            {
                throw new ArgumentNullException("timeZone");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return DateTime.SpecifyKind(TimeZoneInfo.ConvertTimeFromUtc(this.GetUtcDateTime(context), timeZone), DateTimeKind.Local);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Creates a new instance of DbCommand implemented based on the provider type.
        /// </summary>
        protected abstract DbCommand CreateCommand();

        protected abstract string GetDatabaseServerDateTimeScript();

        #endregion Protected Methods
    }
}