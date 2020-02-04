
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    internal static class DatabaseContextFactory
    {
        #region Internal Methods

        internal static DatabaseContext CreateFromElement(DatabaseContextElement config)
        {
            DatabaseContext databaseContext = new DatabaseContext();
            databaseContext.Name = config.ConnectionStringName;
            databaseContext.Provider = ProviderFactory.CreateFromElement(config);

            return databaseContext;
        }

        internal static DatabaseContext CreateFromValues(ProviderType providerType, string connectionStringName, int commandTimeout)
        {
            DatabaseContext databaseContext = new DatabaseContext();
            databaseContext.Name = connectionStringName;
            databaseContext.Provider = ProviderFactory.CreateFromValues(commandTimeout, connectionStringName, providerType);

            return databaseContext;
        }

        #endregion Internal Methods
    }
}
