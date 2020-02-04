
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using CarpathianMadness.Framework;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Represents a database that can be queried against.
    /// </summary>
    public sealed class DatabaseContext
    {
        #region Members

        private static DatabaseContextDictionary _databaseContexts = new DatabaseContextDictionary();

        #endregion Members

        #region Constants

        private const string ConfigSectionName = "CarpathianMadness.Framework.DAL";

        #endregion Constants

        #region Properties

        /// <summary>
        /// Get the name of the database context.
        /// </summary>
        public string Name
        {
            get;
            internal set;
        }

        /// <summary>
        /// Get the provider instance for this database context.
        /// </summary>
        public Provider Provider
        {
            get;
            internal set;
        }

        /// <summary>
        /// Get the default database context to be used.
        /// </summary>
        public static DatabaseContext DefaultDatabaseContext
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the collection of available database contexts to use.
        /// </summary>
        public static DatabaseContextDictionary DatabaseContexts
        {
            get
            {
                return _databaseContexts;
            }

            private set
            {
                _databaseContexts = value;
            }
        }

        #endregion Properties

        #region Constructors

        internal DatabaseContext()
        {

        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Adds database contexts provided in the running application's configuration file.
        /// </summary>
        public static void SetupDatabaseContextsFromConfig()
        {
            SetupDatabaseContexts();
        }

        /// <summary>
        /// Adds a new database context using the provided values.
        /// </summary>
        public static void AddDatabaseContext(ProviderType type, string connectionStringName)
        {
            AddDatabaseContext(type, connectionStringName, 60);
        }

        /// <summary>
        /// Adds a new database context using the provided values.
        /// </summary>
        public static void AddDatabaseContext(ProviderType type, string connectionStringName, int commandTimeout)
        {
            //TODO: Validation.
            DatabaseContext databaseContext = DatabaseContextFactory.CreateFromValues(type, connectionStringName, commandTimeout);

            if (_databaseContexts.ContainsKey(connectionStringName))
            {
                throw new DatabaseContextAlreadyDefinedException("There is already a DatabaseContext defined by the name '" + connectionStringName + "'.");
            }

            _databaseContexts.Add(databaseContext.Name, databaseContext);

            if (DefaultDatabaseContext == null)
            {
                DefaultDatabaseContext = databaseContext;
            }
        }

        /// <summary>
        /// Assigns the DatabaseContext matching the provided name
        /// as the default.
        /// </summary>
        public static void SetDefaultDatabaseContext(string name)
        {
            if (!_databaseContexts.ContainsKey(name))
            {
                throw new DatabaseContextsNotDefinedException("There is no DatabaseContext defined by the name '" + name + "'.");
            }

            DefaultDatabaseContext = _databaseContexts[name];
        }


        /// <summary>
        /// Returns the request-response roundtrip time of the database server.
        /// </summary>
        /// <returns>The roundtrip time in milliseconds.</returns>
        public int Ping()
        {
            var t0 = DateTime.UtcNow;
            //var dt = this.GetUtcDateTime();
            var t1 = DateTime.UtcNow;
            return (int)((t1 - t0).TotalMilliseconds);
        }


        /// <summary>
        /// Returns the current utc date/time from the database server.
        /// </summary>
        public DateTime GetUtcDateTime()
        {
            var query = (QueryContext)null;
            try
            {
                query = new QueryContext();
                return this.GetUtcDateTime(query);
            }
            finally
            {
                if (query != null)
                    query.Dispose();
            }
        }



        public DateTime GetUtcDateTime(QueryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return this.Provider.GetUtcDateTime(context);
        }

        public DateTime GetLocalDateTime(QueryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return this.Provider.GetLocalTime(context);
        }

        public DateTime GetLocalDateTime(TimeZoneInfo timeZone, QueryContext context)
        {
            if (timeZone == null)
            {
                throw new ArgumentNullException("timeZone");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return this.Provider.GetLocalTime(timeZone, context);
        }

        #endregion Public Methods

        #region Private Methods

        private static void SetupDatabaseContexts()
        {
            if (DatabaseContexts.Count == 0)
            {
                DatabaseContextDictionary contexts = new DatabaseContextDictionary();

                DatabaseContextConfig databaseContextConfig = (DatabaseContextConfig)ConfigurationManager.GetSection(ConfigSectionName);


                if (databaseContextConfig != null && databaseContextConfig.DatabaseContexts.Count > 0)
                {
                    for (int i = 0; i < databaseContextConfig.DatabaseContexts.Count; i++)
                    {
                        DatabaseContextElement element = databaseContextConfig.DatabaseContexts[i];
                        DatabaseContext databaseContext = DatabaseContextFactory.CreateFromElement(element);

                        contexts.Add(databaseContext.Name, databaseContext);

                        if (DefaultDatabaseContext == null && element.IsDefault)
                        {
                            DefaultDatabaseContext = databaseContext;
                        }
                    }

                    DatabaseContexts = contexts;

                    if (DefaultDatabaseContext == null)
                    {
                        DefaultDatabaseContext = contexts.Values.ElementAt(0);
                    }
                }
            }
        }

        #endregion Private Methods

    }
}