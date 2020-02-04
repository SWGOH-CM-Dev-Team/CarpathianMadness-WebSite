
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    [ConfigurationCollection(typeof(DatabaseContextElement))]
    public sealed class DatabaseContextElementCollection : ConfigurationElementCollection
    {
        #region Properties

        public DatabaseContextElement this[int index]
        {
            get
            {
                return (DatabaseContextElement)base.BaseGet(index);
            }
        }

        #endregion Properties

        #region Protected Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new DatabaseContextElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatabaseContextElement)element).ConnectionStringName;
        }

        #endregion Protected Methods
    }
}
