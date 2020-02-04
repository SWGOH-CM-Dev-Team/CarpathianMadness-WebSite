
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Implementation of Provider to be used with Microsoft Sql Server.
    /// Only tested with 2008 but should work with 2000 and 2005 engines.
    /// </summary>
    public sealed class SqlClientProvider : Provider
    {
        #region Constructors

        internal SqlClientProvider(int commandTimeout, ConnectionStringSettings connectionStringSettings) : base(commandTimeout, connectionStringSettings)
        {

        }

        #endregion Constructors

        #region Public Methods

        public override DbConnection CreateConnection()
        {
            return new SqlConnection(this.ConnectionStringSettings.ConnectionString);
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public override DbParameter CreateParameter()
        {
            return new SqlParameter();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override DbCommand CreateCommand()
        {
            return new SqlCommand();
        }

        protected override string GetDatabaseServerDateTimeScript()
        {
            throw new NotImplementedException();
        }

        #endregion Protected Methods
    }
}
