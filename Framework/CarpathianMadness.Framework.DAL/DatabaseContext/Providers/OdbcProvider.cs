
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class OdbcProvider : Provider
    {
        #region Constructors

        internal OdbcProvider(int commandTimeout, ConnectionStringSettings connectionStringSettings) : base(commandTimeout, connectionStringSettings)
        {

        }

        #endregion Constructors

        #region Public Methods

        public override DbConnection CreateConnection()
        {
            return new OdbcConnection(this.ConnectionStringSettings.ConnectionString);
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new OdbcDataAdapter();
        }

        public override DbParameter CreateParameter()
        {
            return new OdbcParameter();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override DbCommand CreateCommand()
        {
            return new OdbcCommand();
        }

        protected override string GetDatabaseServerDateTimeScript()
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}
