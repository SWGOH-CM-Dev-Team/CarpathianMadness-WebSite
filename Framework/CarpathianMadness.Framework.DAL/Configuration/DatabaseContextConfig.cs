
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class DatabaseContextConfig : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("DatabaseContexts")]
        public DatabaseContextElementCollection DatabaseContexts
        {
            get
            {
                return (DatabaseContextElementCollection)base["DatabaseContexts"];
            }
        }

        #endregion Properties
    }
}
