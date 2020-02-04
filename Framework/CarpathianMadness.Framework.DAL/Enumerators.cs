
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    #region DatabaseContext

    /// <summary>
    /// Defines the ProviderType, which will
    /// affect what implementations of DbConnection, DbCommand
    /// etc are created when performing queries to a database.
    /// </summary>
    public enum ProviderType : byte
    {
        /// <summary>
        /// Sql Server 2008 (Tested) but should work for 2005 as well.
        /// </summary>
        SqlServer = 1,

        /// <summary>
        /// Postgresql 9.2 (Tested) although should work for any minor version under 9.
        /// </summary>
        PostgreSql = 2,

        /// <summary>
        /// Uses the Open Database Connectivity implementations to talk to a database.
        /// </summary>
        ODBC = 3
    }

    /// <summary>
    /// Defines the SaveType depending if the framework
    /// is saving a new entity instance or an existing one.
    /// For existing elements, you'll normally need to provide
    /// the Primary Key of an entity.
    /// </summary>
    public enum SaveType : byte
    {
        Create = 0,
        Update = 1
    }

    #endregion DatabaseContext
}
