
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public static class EntityFactory<TEntity> where TEntity : Entity<TEntity>, new()
    {
        #region Public Methods

        public static TEntity CreateFromReader(DbDataReader reader)
        {
            TEntity instance = new TEntity();
            instance.InternalLoad(reader);

            return instance;
        }

        public static TEntity CreateFromDataRow(DataRow row)
        {
            TEntity entity = null;

            using (DbDataReader reader = row.Table.CreateDataReader())
            {
                if (reader.Read())
                {
                    entity = CreateFromReader(reader);
                }
            }

            return entity;
        }

        public static IList<TEntity> CreateListFromDataTable(DataTable table)
        {
            return CreateListFromReader(table.CreateDataReader());
        }

        public static IList<TEntity> CreateListFromReader(DbDataReader reader)
        {
            IList<TEntity> entities = new List<TEntity>();

            while (reader.Read())
            {
                entities.Add(CreateFromReader(reader));
            }

            return entities;
        }

        #endregion Public Methods
    }
}