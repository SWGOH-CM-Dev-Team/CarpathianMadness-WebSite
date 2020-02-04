using System;
using System.Collections.Generic;
using System.Text;

namespace CarpathianMadness.Services
{
    public class CarpathianMadnessService : ICarpathianMadnessService
    {
        public string Key => throw new NotImplementedException();

        public string Message => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public void AddError(string key, string message)
        {
            throw new NotImplementedException();
        }

        public void AddWarning(string key, string message)
        {
            throw new NotImplementedException();
        }

        public bool RegionAuthorise(int UserId, int regionId)
        {
            throw new NotImplementedException();
        }
    }
}
