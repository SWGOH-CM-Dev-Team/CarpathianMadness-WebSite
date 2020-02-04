
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Check if the DataSet instance has tables with rows.
        /// </summary>
        public static bool IsEmpty(this DataSet dataSet)
        {
            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            bool outcome = true;

            if (dataSet.Tables.Count > 0)
            {
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    DataTable table = dataSet.Tables[i];

                    if (table.Rows.Count > 0)
                    {
                        outcome = false;
                        break;
                    }
                }
            }

            return outcome;
        }

        #endregion Public Methods
    }
}