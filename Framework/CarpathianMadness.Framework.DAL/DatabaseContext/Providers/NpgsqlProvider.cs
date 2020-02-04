
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using Npgsql;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class NpgsqlProvider : Provider
    {
        #region Constants

        private const string _UTCSelect = "SELECT timezone('UTC', now());";

        #endregion Constants

        #region Constructors

        internal NpgsqlProvider(int commandTimeout, ConnectionStringSettings connectionStringSettings)
            : base(commandTimeout, connectionStringSettings)
        {

        }

        #endregion Constructors

        #region Public Methods

        public override DbConnection CreateConnection()
        {
            //HACK: Npgsql driver defaults to setting MAXPOOLSIZE to 20 if pooling is used, default this to 200.
            var builder = new NpgsqlConnectionStringBuilder(this.ConnectionStringSettings.ConnectionString);

            if (builder.Pooling && builder.MaxPoolSize == 20)
            {
                builder.MaxPoolSize = 200;
            }

            if (string.IsNullOrWhiteSpace(builder.ApplicationName))
            {
                var assembly = System.Reflection.Assembly.GetEntryAssembly();
                builder.ApplicationName = "Test"; //AssemblyUtility.AssemblyProduct(assembly) + " " + AssemblyUtility.AssemblyDisplayVersion(assembly);
            }

            var connection = new NpgsqlConnection(builder.ToString());

            return connection;
        }

        public override DbCommand CreateCommand(string commandText, CommandType commandType, DbConnection connection)
        {
            DbCommand command = base.CreateCommand(commandText, commandType, connection);

            //if(commandType == CommandType.StoredProcedure)
            //{
            //    command.CommandText = "\"" + command.CommandText + "\"";
            //}

            return command;
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new NpgsqlDataAdapter();
        }

        public override DbParameter CreateParameter()
        {
            return new NpgsqlParameter();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override DbCommand CreateCommand()
        {
            return new NpgsqlCommand();
        }

        protected override string GetDatabaseServerDateTimeScript()
        {
            return _UTCSelect;
        }

        #endregion Protected Methods
    }
}