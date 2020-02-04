using CarpathianMadness.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Text;

namespace CarpathianMadness.Business
{
    [DataContract]
    public sealed class HomeSearch : BasicEntity<HomeSearch>
    {
        // add propeties here

        [DataMember]
        public int ID { get; private set; }


        protected override void OnLoading(DbDataReader reader)
        {
            this.ID = (int)reader["ID"];            
        }

        protected override void OnSaving(DatabaseContext context, CommandParameterCollection parameters, SaveType saveType)
        {
            throw new NotImplementedException();
        }
    }
}
