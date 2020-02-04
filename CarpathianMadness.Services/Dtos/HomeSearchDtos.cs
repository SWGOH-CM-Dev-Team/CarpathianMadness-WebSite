using CarpathianMadness.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CarpathianMadness.Services
{
    [DataContract]
   public class HomeSearchDtos
   {
        [DataMember]
        public long ID { get; private set; }



        public List<HomeSearchDtos> GetData(IList<HomeSearch> item)
        {
            var result = (from i in item
                          select new HomeSearchDtos()
                          {
                              ID = i.ID      
                          }).ToList();

            return result;
        }


    }
}
