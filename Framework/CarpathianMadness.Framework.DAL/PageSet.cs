
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public sealed class PageSet<TEntity> where TEntity : Entity<TEntity>, new()
    {
        #region Properties

        public ReadOnlyCollection<TEntity> Items
        {
            get;
            private set;
        }

        public int PageIndex
        {
            get;
            private set;
        }

        public int PageSize
        {
            get;
            private set;
        }

        public int RowCount
        {
            get;
            private set;
        }

        public int PageCount
        {
            get;
            private set;
        }

        #endregion Properties

        #region Constructors

        internal PageSet(IList<TEntity> items, int pageIndex, int pageSize, int rowCount)
        {
            this.Items = new ReadOnlyCollection<TEntity>(items);
            this.PageIndex = PageIndex;
            this.PageSize = PageSize;
            this.RowCount = rowCount;

            if (this.PageSize > 0 && this.RowCount > 0)
            {
                this.PageCount = this.RowCount / this.PageSize;

                if (this.RowCount % this.PageSize > 0)
                {
                    this.PageCount++;
                }
            }
        }

        #endregion Constructors
    }
}