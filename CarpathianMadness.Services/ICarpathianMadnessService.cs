using System;
using System.Collections.Generic;
using System.Text;

namespace CarpathianMadness.Services
{
    public interface ICarpathianMadnessService
    {
        /// <summary>
        /// test stuf
        /// </summary>

        string Key { get; }
        string Message { get; }
        bool IsValid { get; }
        void AddError(string key, string message);
        void AddWarning(string key, string message);
        bool RegionAuthorise(int UserId, int regionId);
    }
}
