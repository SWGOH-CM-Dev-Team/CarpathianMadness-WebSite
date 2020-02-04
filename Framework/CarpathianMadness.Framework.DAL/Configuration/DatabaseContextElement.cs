
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class DatabaseContextElement : ConfigurationElement
    {
        #region Properties

        [ConfigurationProperty("ConnectionStringName", IsRequired = true, IsKey = true)]
        public string ConnectionStringName
        {
            get
            {
                return (string)base["ConnectionStringName"];
            }

            set
            {
                base["ConnectionStringName"] = value;
            }
        }

        [ConfigurationProperty("ProviderType", IsRequired = true)]
        public ProviderType ProviderType
        {
            get
            {
                return (ProviderType)base["ProviderType"];
            }

            set
            {
                base["ProviderType"] = value;
            }
        }

        [ConfigurationProperty("CommandTimeout", DefaultValue = 60)]
        [IntegerValidator(MinValue = 60)]
        public int CommandTimeout
        {
            get
            {
                return (int)base["CommandTimeout"];
            }

            set
            {
                base["CommandTimeout"] = value;
            }
        }

        [ConfigurationProperty("IsDefault", DefaultValue = false)]
        public bool IsDefault
        {
            get
            {
                return (bool)base["IsDefault"];
            }

            set
            {
                base["IsDefault"] = value;
            }
        }

        #endregion Properties
    }
}
