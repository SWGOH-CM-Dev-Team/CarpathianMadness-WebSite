
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public static class EntityLayer<TEntity> where TEntity : Entity<TEntity>, new()
    {
        #region Public Methods

        #region Single

        [Obsolete("Use ExecuteSingle<TEntity> using the QueryContext instance.")]
        public static TEntity ExecuteSingle(QueryContext query, CommandParameterCollection parameters, string procedureName)
        {
            return BaseEntityLayer<TEntity>.ExecuteSingle(query, parameters, procedureName);
        }

        #endregion Single

        #region List

        [Obsolete("Use ExecuteList<TEntity> using the QueryContext instance.")]
        public static IList<TEntity> ExecuteList(DatabaseContext context, string procedureName)
        {
            return BaseEntityLayer<TEntity>.ExecuteList(context, procedureName, null);
        }

        [Obsolete("Use ExecuteList<TEntity> using the QueryContext instance.")]
        public static IList<TEntity> ExecuteList(DatabaseContext context, string procedureName, CommandParameterCollection parameters)
        {
            return BaseEntityLayer<TEntity>.ExecuteList(context, procedureName, parameters);
        }

        #endregion List

        //#region PageSet

        //public static PageSet<TEntity> ExecutePageSet(DatabaseContext context, string procedureName, int pageIndex, int pageSize, CommandParameterCollection parameters)
        //{
        //    return BaseEntityLayer<TEntity>.ExecutePageSet(context, procedureName, pageIndex, pageSize, parameters);
        //}

        //#endregion PageSet

        #endregion Public Methods
    }
}
