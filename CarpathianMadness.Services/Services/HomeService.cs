using CarpathianMadness.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpathianMadness.Services
{
    public class HomeService : IHomeService
    {
        public IList<HomeSearchDtos> SearchHome(long id)
        {
            HomeSearchDtos home = new HomeSearchDtos();
            var hometest = home.GetData(Home_Layer.SearchHome(id));

            return hometest;
        }
    }
}
