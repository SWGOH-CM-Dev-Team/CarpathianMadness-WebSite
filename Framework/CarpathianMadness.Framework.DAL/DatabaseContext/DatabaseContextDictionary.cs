
using System.Collections.Generic;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Represents a dictionary of database contexts.
    /// </summary>
    public sealed class DatabaseContextDictionary : Dictionary<string, DatabaseContext>
    {
        #region Constructors

        internal DatabaseContextDictionary() : base()
        {

        }

        // DJ: Unused
        //internal DatabaseContextDictionary(IDictionary<string, DatabaseContext> dictionary) : base(dictionary)
        //{
        //}

        #endregion Constructors
    }
}
