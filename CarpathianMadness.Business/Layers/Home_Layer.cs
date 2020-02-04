using CarpathianMadness.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CarpathianMadness.Business
{
    public static class Home_Layer
    {

        /// <summary>
        /// customer search
        /// </summary>
        public static IList<HomeSearch> SearchHome(long id)
        {
            using (QueryContext query = new QueryContext())
            {
                CommandParameterCollection parameters = new CommandParameterCollection();
                parameters.AddValue("id", id, DbType.Int64);    
                return query.ExecuteList<HomeSearch>("public.usp_2018_customers_search", CommandType.StoredProcedure, parameters);
            }
        }
    }
}
