
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    public static class BasicEntityLayer<TEntity> where TEntity : BasicEntity<TEntity>, new()
    {
        #region Public Methods

        #region Validation

        /// <summary>
        /// Validates the provided entity.
        /// </summary>
        /// <param name="entity">The entity to validate.</param>
        /// <returns>Collection validation errors.</returns>
        public static ErrorCollection Validate(TEntity entity)
        {
            ErrorCollection errors = new ErrorCollection();

            if (entity != null)
            {
                entity.InternalValidate(errors);
            }

            return errors;
        }

        #endregion Validation

        #region Save

        /// <summary>
        /// Saves changed to the Database for the provided entity.
        /// </summary>
        public static ErrorCollection ExecuteSave(QueryContext query, TEntity entity, string createProcedureName, string updateProcedureName)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query cannot be null.");
            }

            if (entity == null)
            {
                throw new ArgumentNullException("Entity(" + typeof(TEntity).Name + ") cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(createProcedureName))
            {
                throw new ArgumentException("createProcedureName cannot be null, empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(updateProcedureName))
            {
                throw new ArgumentException("updateProcedureName cannot be null, empty or whitespace.");
            }

            ErrorCollection errors;

            if (entity.IsNew)
            {
                errors = ExecuteCreate(query, entity, createProcedureName);
            }
            else
            {
                errors = ExecuteUpdate(query, entity, updateProcedureName);
            }

            return errors;
        }

        /// <summary>
        /// Saves changed to the Database for the provided entity.
        /// </summary>
        public static ErrorCollection ExecuteCreate(QueryContext query, TEntity entity, string procedureName)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query cannot be null.");
            }

            if (entity == null)
            {
                throw new ArgumentNullException("Entity(" + typeof(TEntity).Name + ") cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException("procedureName cannot be null, empty or whitespace.");
            }

            if (!entity.IsNew)
            {
                throw new ArgumentException("Entity must be new.");
            }

            ErrorCollection errors = new ErrorCollection();
            entity.InternalValidate(errors);

            if (errors.Count == 0)
            {
                SaveType saveType = SaveType.Create;
                CommandParameterCollection parameters = new CommandParameterCollection();
                entity.InternalSave(query.DatabaseContext, parameters, saveType);

                ExecuteQueryUpdateModel(query, entity, procedureName, parameters);
            }

            return errors;
        }

        /// <summary>
        /// Saves changes to the Database for the provided entity using the given parameters.
        /// </summary>
        public static ErrorCollection ExecuteCreate(QueryContext query, TEntity entity, string procedureName, CommandParameterCollection parameters)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (string.IsNullOrWhiteSpace(procedureName))
                throw new ArgumentException("procedureName");

            if (parameters == null || parameters.Count == 0)
            {
                throw new ArgumentException("At least 1 parameter must be specified.");
            }

            if (!entity.IsNew)
            {
                throw new ArgumentException("Entity must be new.");
            }

            ErrorCollection errors = new ErrorCollection();
            entity.InternalValidate(errors);

            if (errors.Count == 0)
            {
                ExecuteQueryUpdateModel(query, entity, procedureName, parameters);
            }

            return errors;
        }

        /// <summary>
        /// Saves changed to the Database for the provided entity.
        /// </summary>
        public static ErrorCollection ExecuteUpdate(QueryContext query, TEntity entity, string procedureName)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException("procedureName cannot be null, empty or whitespace.");
            }

            if (entity.IsNew)
            {
                throw new ArgumentException("Entity must not be new.");
            }

            ErrorCollection errors = new ErrorCollection();
            entity.InternalValidate(errors);

            if (errors.Count == 0)
            {
                SaveType saveType = SaveType.Update;
                CommandParameterCollection parameters = new CommandParameterCollection();
                entity.InternalSave(query.DatabaseContext, parameters, saveType);

                ExecuteQueryUpdateModel(query, entity, procedureName, parameters);
            }

            return errors;
        }

        /// <summary>
        /// Saves changes for the provided collection of entities.
        /// Any entities failing validation will not be saved and will added to the returning IDictionary.
        /// </summary>
        public static IDictionary<int, ErrorCollection> ExecuteSave(QueryContext query, IList<TEntity> entities, string createProcedureName, string updateProcedureName)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query cannot be null.");
            }

            if (entities == null)
            {
                throw new ArgumentNullException("Entities(" + typeof(TEntity).Name + ") list cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(createProcedureName))
            {
                throw new ArgumentException("createProcedureName cannot be null, empty or whitespace.");
            }

            if (string.IsNullOrWhiteSpace(updateProcedureName))
            {
                throw new ArgumentException("updateProcedureName cannot be null, empty or whitespace.");
            }

            IDictionary<int, ErrorCollection> errors = new Dictionary<int, ErrorCollection>();

            if (entities.Count > 0)
            {
                for (int i = 0; i < entities.Count; i++)
                {
                    ErrorCollection entityErrors = ExecuteSave(query, entities[i], createProcedureName, updateProcedureName);

                    if (entityErrors.Count > 0)
                    {
                        //Record the index of the entity in the collection and it's list of error strings.
                        errors.Add(i, entityErrors);
                    }
                }
            }

            return errors;
        }

        #endregion Save

        #region Delete

        public static bool ExecuteDelete(QueryContext query, TEntity entity, string procedureName)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query cannot be null.");
            }

            if (entity == null)
            {
                throw new ArgumentNullException("Entity(" + typeof(TEntity).Name + ") cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException("procedureName cannot be null, empty or whitespace.");
            }

            CommandParameterCollection parameters = new CommandParameterCollection();
            entity.InternalDelete(parameters);

            return ExecuteQueryDeleteModel(query, entity, procedureName, parameters);
        }

        #endregion Delete

        #endregion Public Methods

        #region Private Methods

        private static void ExecuteQueryUpdateModel(QueryContext query, TEntity entity, string procedureName, CommandParameterCollection parameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (parameters.Count == 0)
            {
                throw new InvalidOperationException("Unable to execute '" + procedureName + "' on Entity(" + typeof(TEntity).Name + ") if no parameters are provided...");
            }

            using (DbDataReader reader = query.ExecuteReader(procedureName, CommandType.StoredProcedure, parameters))
            {
                if (reader.Read())
                {
                    entity.InternalLoad(reader);
                }
            }
        }

        private static bool ExecuteQueryDeleteModel(QueryContext query, TEntity entity, string procedureName, CommandParameterCollection parameters)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (parameters.Count == 0)
            {
                throw new InvalidOperationException("Unable to execute '" + procedureName + "' on Entity(" + typeof(TEntity).Name + ") if no parameters are provided...");
            }

            return query.ExecuteScalar<bool>(procedureName, CommandType.StoredProcedure, parameters);
        }

        #endregion Private Methods
    }
}
