using System;
using System.Collections.Generic;
using System.Text;

namespace CarpathianMadness.Services
{
    public interface IHomeService
    {
        IList<HomeSearchDtos> SearchHome(long id);
    }
}
