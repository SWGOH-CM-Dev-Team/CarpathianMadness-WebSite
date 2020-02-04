
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    internal static class ProviderFactory
    {
        #region Internal Methods

        internal static Provider CreateFromElement(DatabaseContextElement config)
        {
            return CreateFromValues(config.CommandTimeout, config.ConnectionStringName, config.ProviderType);
        }

        internal static Provider CreateFromValues(int commandTimeout, string connectionStringName, ProviderType providerType)
        {
            Provider provider;
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (connectionStringSettings == null)
            {
                throw new NullReferenceException("connectionStringSettings");
            }

            switch (providerType)
            {
                case ProviderType.SqlServer:
                    {
                        provider = new SqlClientProvider(commandTimeout, connectionStringSettings);
                        break;
                    }

                case ProviderType.PostgreSql:
                    {
                        provider = new NpgsqlProvider(commandTimeout, connectionStringSettings);
                        break;
                    }

                case ProviderType.ODBC:
                    {
                        provider = new OdbcProvider(commandTimeout, connectionStringSettings);
                        break;
                    }

                default:
                    {
                        throw new NotSupportedException("Unsupported ProviderType: " + providerType.ToString());
                    }
            }

            return provider;
        }

        #endregion Internal Methods
    }
}
