
using System;
using System.Collections.Generic;
using System.Data;

namespace CarpathianMadness.Framework.DAL
{
    internal static class BaseEntityLayer<TEntity> where TEntity : Entity<TEntity>, new()
    {
        #region Internal Methods

        #region Single

        [Obsolete("Use ExecuteSingle<TEntity> using the QueryContext instance.")]
        internal static TEntity ExecuteSingle(QueryContext query, CommandParameterCollection parameters)
        {
            return ExecuteSingle(query, parameters, typeof(TEntity).Name + "_Find");
        }

        [Obsolete("Use ExecuteSingle<TEntity> using the QueryContext instance.")]
        internal static TEntity ExecuteSingle(QueryContext query, CommandParameterCollection parameters, string procedureName)
        {
            TEntity entity = query.ExecuteSingle<TEntity>(procedureName, CommandType.StoredProcedure, parameters);
            return entity;
        }

        #endregion Single

        #region List

        [Obsolete("Use ExecuteList<TEntity> using the QueryContext instance.")]
        internal static IList<TEntity> ExecuteList(DatabaseContext context)
        {
            return ExecuteList(context, typeof(TEntity).Name + "_List", null);
        }

        [Obsolete("Use ExecuteList<TEntity> using the QueryContext instance.")]
        internal static IList<TEntity> ExecuteList(DatabaseContext context, string procedureName)
        {
            return ExecuteList(context, procedureName, null);
        }

        [Obsolete("Use ExecuteList<TEntity> using the QueryContext instance.")]
        internal static IList<TEntity> ExecuteList(DatabaseContext context, CommandParameterCollection parameters)
        {
            return ExecuteList(context, typeof(TEntity).Name + "_List", parameters);
        }

        [Obsolete("Use ExecuteList<TEntity> using the QueryContext instance.")]
        internal static IList<TEntity> ExecuteList(DatabaseContext context, string procedureName, CommandParameterCollection parameters)
        {
            IList<TEntity> entities;

            using (QueryContext query = new QueryContext(context))
            {
                entities = query.ExecuteList<TEntity>(procedureName, CommandType.StoredProcedure, parameters);
            }

            return entities;
        }

        #endregion List

        //#region PageSet

        //internal static PageSet<TEntity> ExecutePageSet(DatabaseContext context, int pageIndex, int pageSize, CommandParameterCollection parameters)
        //{
        //    return ExecutePageSet(context, typeof(TEntity).Name + "_PageSet", pageIndex, pageSize, parameters);
        //}

        //internal static PageSet<TEntity> ExecutePageSet(DatabaseContext context, string procedureName, int pageIndex, int pageSize, CommandParameterCollection parameters)
        //{
        //    IList<TEntity> entities;
        //    int rowCount;

        //    using(QueryContext query = new QueryContext(context))
        //    {
        //        using(DbDataReader reader = query.ExecuteReader(procedureName, CommandType.StoredProcedure, parameters))
        //        {
        //            entities = EntityFactory<TEntity>.CreateListFromReader(reader);
        //            rowCount = reader.GetValueOrDefault<int>("RowCount");
        //        }
        //    }

        //    return new PageSet<TEntity>(entities, pageIndex, pageSize, rowCount);
        //}

        //#endregion PageSet

        #endregion Internal Methods
    }
}
