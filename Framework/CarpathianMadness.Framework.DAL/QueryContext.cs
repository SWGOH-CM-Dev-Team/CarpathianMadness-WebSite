
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

using CarpathianMadness.Framework;
using CarpathianMadness.Framework.NLog;

using NLog;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Query context object that handles running queries
    /// on a database.
    /// </summary>
    public sealed class QueryContext : IDisposable
    {
        #region Members

        private DbConnection _connection = null;
        private DbTransaction _transaction = null;
        private Exception _exception = null;

        #endregion Members

        #region Constants

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion Constants

        #region Properties

        /// <summary>
        /// Get/Set the CommandTimeout of a query.
        /// </summary>
        public int CommandTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Get the DatabaseContext of this instance.
        /// </summary>
        public DatabaseContext DatabaseContext
        {
            get;
            private set;
        }

        /// <summary>
        /// Get the boolean indicating if this query is currently running
        /// under a transaction.
        /// </summary>
        public bool IsAtomic
        {
            get;
            private set;
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Creates a new QueryContext instance and will use the DefaultDatabaseContext to run against.
        /// </summary>
        public QueryContext() : this(DatabaseContext.DefaultDatabaseContext)
        {

        }

        /// <summary>
        /// Creates a new QueryContext instance and will use the provided DatabaseContext to run against.
        /// </summary>
        public QueryContext(DatabaseContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.DatabaseContext = context;
            this.CommandTimeout = context.Provider.CommandTimeout;
        }

        /// <summary>
        /// Creates a new QueryContext instance and will use the DatabaseContext matching the provided contextName to run against.
        /// </summary>
        /// <param name="contextName">The name of the DatabaseContext</param>
        public QueryContext(string contextName)
            : this(DatabaseContext.DatabaseContexts[contextName])
        {
        }

        public static QueryContext CreateFromDefault()
        {
            return new QueryContext();
        }

        #endregion Constructors

        #region Deconstructors

        ~QueryContext()
        {
            this.Dispose(false);
        }

        #endregion Deconstructors

        #region Public Methods

        #region Transaction

        /// <summary>
        /// Begins a transaction using the default Isolation level, which
        /// varies depending on the database context.
        /// </summary>
        public void StartTransaction()
        {
            if (this._transaction == null)
            {
                CheckConnection();
                this._transaction = this._connection.BeginTransaction();
                this.IsAtomic = true;
            }
        }

        /// <summary>
        /// Begins a transaction using the provided Isolation level.
        /// </summary>
        public void StartTransaction(IsolationLevel level)
        {
            if (this._transaction == null)
            {
                CheckConnection();
                this._transaction = this._connection.BeginTransaction(level);
                this.IsAtomic = true;
            }
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (this._transaction != null)
            {
                this._transaction.Commit();
                this._transaction.Dispose();
                this._transaction = null;

                this.IsAtomic = false;
            }
        }

        /// <summary>
        /// Rolls back the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (this._transaction != null)
            {
                this._transaction.Rollback();
                this._transaction.Dispose();
                this._transaction = null;

                this.IsAtomic = false;
            }
        }

        #endregion Transaction

        #region Execute Reader

        /// <summary>
        /// Executes a query to the database and returns the result as a DbDataReader.
        /// </summary>
        public DbDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return ExecuteReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as a DbDataReader.
        /// </summary>
        public DbDataReader ExecuteReader(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            DbDataReader reader = null;
            DbCommand command = null;

            try
            {
                CheckConnection();

                command = SetupDbCommand(this, this.DatabaseContext, this._connection, this._transaction, commandText, commandType, parameters);

                //if(this._transaction != null)
                //{
                //	reader = command.ExecuteReader();
                //}
                //else
                //{
                //	reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                //}

                reader = command.ExecuteReader();

                ProcessOutputParameters(command, parameters);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
            finally
            {
                DisposeUtility.Dispose(command);
            }

            return reader;
        }

        #endregion Execute Reader

        #region Execute DataSet/DataTable/DataRow

        /// <summary>
        /// Executes a query to the database and returns the result as a DataSet.
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return ExecuteDataSet(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as a DataSet.
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            DataSet set = null;
            DbCommand command = null;
            DbDataAdapter adapter = null;

            try
            {
                CheckConnection();

                command = SetupDbCommand(this, this.DatabaseContext, this._connection, this._transaction, commandText, commandType, parameters);

                adapter = this.DatabaseContext.Provider.CreateDataAdapter();
                adapter.SelectCommand = command;

                set = new DataSet();
                adapter.Fill(set);

                //CloseConnection();

                ProcessOutputParameters(command, parameters);
            }
            catch (Exception ex)
            {
                if (set != null)
                {
                    set.Dispose();
                }

                HandleException(ex);
                throw;
            }
            finally
            {
                DisposeUtility.Dispose(adapter, command);
            }

            return set;
        }

        /// <summary>
        /// Executes a query to the database and returns the result as a DataTable.
        /// </summary>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return ExecuteDataTable(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as a DataTable.
        /// </summary>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            DataTable table = null;
            DataSet set = ExecuteDataSet(commandText, commandType, parameters);

            if (set != null && set.Tables.Count > 0)
            {
                table = set.Tables[0];
            }

            return table;
        }

        /// <summary>
        /// Executes a query to the database and returns the result as a DataRow.
        /// </summary>
        public DataRow ExecuteDataRow(string commandText, CommandType commandType)
        {
            return ExecuteDataRow(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as a DataRow.
        /// </summary>
        public DataRow ExecuteDataRow(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            DataRow row = null;
            DataSet set = ExecuteDataSet(commandText, commandType, parameters);

            if (set != null && set.Tables.Count > 0)
            {
                DataTable table = set.Tables[0];

                if (table.Rows.Count > 0)
                {
                    row = table.Rows[0];
                }
            }

            return row;
        }

        #endregion Execute DataSet/DataTable/DataRow

        #region Scalar

        /// <summary>
        /// Executes a query to the database and returns the result as an object.
        /// </summary>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as an object.
        /// </summary>
        public object ExecuteScalar(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            object scalarValue = null;
            DbCommand command = null;

            try
            {
                CheckConnection();

                command = SetupDbCommand(this, this.DatabaseContext, this._connection, this._transaction, commandText, commandType, parameters);

                scalarValue = command.ExecuteScalar();

                //CloseConnection();

                ProcessOutputParameters(command, parameters);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
            finally
            {
                DisposeUtility.Dispose(command);
            }

            return scalarValue;
        }

        /// <summary>
        /// Executes a query to the database and returns the result as a typed object.
        /// </summary>
        public TObject ExecuteScalar<TObject>(string commandText, CommandType commandType)
        {
            return ExecuteScalar<TObject>(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as a typed object.
        /// </summary>
        public TObject ExecuteScalar<TObject>(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            TObject outcome;
            object value = ExecuteScalar(commandText, commandType, parameters);

            if (value != DBNull.Value)
            {
                //outcome = (TObject)value;
                outcome = TryCast.AsGeneric<TObject>(value, null, default(TObject));
            }
            else
            {
                outcome = default(TObject);
            }

            return outcome;
        }

        #endregion Scalar

        #region Non Query

        /// <summary>
        /// Executes a query to the database and returns the result the number of rows affected, if any.
        /// </summary>
        public int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return ExecuteNonQuery(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result the number of rows affected, if any.
        /// </summary>
        public int ExecuteNonQuery(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            int affectedRows = -1;
            DbCommand command = null;
            //IList<DbParameter> outputParameters = new List<DbParameter>();

            try
            {
                CheckConnection();

                command = SetupDbCommand(this, this.DatabaseContext, this._connection, this._transaction, commandText, commandType, parameters);

                affectedRows = command.ExecuteNonQuery();

                //CloseConnection();

                ProcessOutputParameters(command, parameters);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
            finally
            {
                DisposeUtility.Dispose(command);
            }

            return affectedRows;
        }

        #endregion Non Query

        #region Single Entity

        /// <summary>
        /// Executes a query to the database and returns the result as an Entity object.
        /// </summary>
        public TEntity ExecuteSingle<TEntity>(string commandText, CommandType commandType) where TEntity : Entity<TEntity>, new()
        {
            return ExecuteSingle<TEntity>(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as an Entity object.
        /// </summary>
        public TEntity ExecuteSingle<TEntity>(string commandText, CommandType commandType, CommandParameterCollection parameters) where TEntity : Entity<TEntity>, new()
        {
            TEntity entity = default(TEntity);

            DbDataReader reader = null;

            //HACK: Issue with Npgsql DbReader implementation that if no results are returned
            //it'll still return true when invoking reader.Read() which you would have thought
            //it'd return false...
            try
            {
                reader = ExecuteReader(commandText, commandType, parameters);

                if (reader.Read())
                {
                    entity = EntityFactory<TEntity>.CreateFromReader(reader);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                DisposeUtility.Dispose(reader);
            }

            return entity;
        }

        public IDictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(string commandText, CommandType commandType)
        {
            return ExecuteDictionary<TKey, TValue>(commandText, commandType, null);
        }

        public IDictionary<TKey, TValue> ExecuteDictionary<TKey, TValue>(string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            IDictionary<TKey, TValue> results = new Dictionary<TKey, TValue>();
            DbDataReader reader = null;

            //HACK: Issue with Npgsql DbReader implementation that if no results are returned
            //it'll still return true when invoking reader.Read() which you would have thought
            //it'd return false...
            try
            {
                reader = ExecuteReader(commandText, commandType, parameters);

                while (reader.Read())
                {
                    results.Add(reader.GetValue<TKey>(0), reader.GetValue<TValue>(1));
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                DisposeUtility.Dispose(reader);
            }

            return results;
        }

        #endregion Single Entity

        #region List Entity

        /// <summary>
        /// Executes a query to the database and returns the result as collection of Entity objects.
        /// </summary>
        public IList<TEntity> ExecuteList<TEntity>(string commandText, CommandType commandType) where TEntity : Entity<TEntity>, new()
        {
            return ExecuteList<TEntity>(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a query to the database using the provided parameters and returns the result as collection of Entity objects.
        /// </summary>
        public IList<TEntity> ExecuteList<TEntity>(string commandText, CommandType commandType, CommandParameterCollection parameters) where TEntity : Entity<TEntity>, new()
        {
            IList<TEntity> entities;

            DbDataReader reader = null;

            //HACK: Issue with Npgsql DbReader implementation that if no results are returned
            //it'll still return true when invoking reader.Read() which you would have thought
            //it'll return false...
            try
            {
                reader = ExecuteReader(commandText, commandType, parameters);

                entities = EntityFactory<TEntity>.CreateListFromReader(reader);
            }
            catch (Exception ex)
            {
                entities = new List<TEntity>();
                HandleException(ex);
            }
            finally
            {
                DisposeUtility.Dispose(reader);
            }

            return entities;
        }

        #endregion List Entity

        #region Misc

        public DateTime GetUtcDateTime()
        {
            return this.DatabaseContext.GetUtcDateTime(this);
        }

        public DateTime GetLocalDateTime()
        {
            return this.DatabaseContext.GetLocalDateTime(this);
        }

        public DateTime GetLocalDateTime(TimeZoneInfo timeZone)
        {
            return this.DatabaseContext.GetLocalDateTime(timeZone, this);
        }

        #endregion Misc

        /// <summary>
        /// Get the last occurring exception from this instance.
        /// </summary>
        public Exception GetException()
        {
            return this._exception;
        }

        /// <summary>
        /// Disposes the QueryContext instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Private Methods

        #region Connection

        /// <summary>
        /// Sets up a connection if one is not initialised.
        /// </summary>
        private void SetupConnection()
        {
            if (this._connection == null)
            {
                this._connection = this.DatabaseContext.Provider.CreateConnection();
            }
        }

        /// <summary>
        /// Sets up a connection if one is not initialised and opens it.
        /// </summary>
        private void CheckConnection()
        {
            SetupConnection();

            if (this._connection.State == ConnectionState.Closed)
            {
                this._connection.Open();
            }
        }

        /// <summary>
        /// Closes the connection, if possible.
        /// </summary>
        private void CloseConnection()
        {
            if (this._transaction == null)
            {
                if (this._connection != null)
                {
                    this._connection.Close();
                    this._connection = null;
                }
            }
        }

        #endregion Connection

        private void Dispose(bool disposing)
        {
            //If, on dispose, the transaction hasn't been handled, than
            //roll it back before disposing it as not all implementations
            //of DbTransaction handle these when being disposed.
            if (this._transaction != null && this.IsAtomic)
            {
                RollbackTransaction();
            }

            this.CloseConnection();

            DisposeUtility.Dispose(this._connection, this._transaction);

            this._connection = null;
            this._transaction = null;
        }

        /// <summary>
        /// Utility method for creating a DbCommand instance.
        /// </summary>
        private static DbCommand SetupDbCommand(QueryContext query, DatabaseContext context, DbConnection connection, DbTransaction transaction, string commandText, CommandType commandType, CommandParameterCollection parameters)
        {
            DbCommand command = context.Provider.CreateCommand(commandText, commandType, connection);
            command.CommandTimeout = query.CommandTimeout;
            command.Transaction = transaction;

            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToDbParameters(context));
                }
                else
                {
                    throw new InvalidOperationException("A CommandParameterCollection instance was passed in but contained no values, provide a NULL reference instead or provide a collection with at least one value.");
                }
            }

            return command;
        }

        /// <summary>
        /// Utility method for processing output parameters.
        /// </summary>
        private static void ProcessOutputParameters(DbCommand command, CommandParameterCollection commandParameters)
        {
            if (commandParameters != null && command.Parameters.Count > 0)
            {
                var outputValues = new Dictionary<string, object>();

                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    DbParameter parameter = command.Parameters[i];

                    if (parameter.Direction != ParameterDirection.Input)
                    {
                        outputValues.Add(parameter.ParameterName, parameter.Value);
                    }
                }

                commandParameters.OutputValues = outputValues;
            }
        }

        private void HandleException(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            //HACK: Do not log InvalidCastExceptions due to empty row.
            if (!(ex is InvalidCastException))
            {
                _logger.LogEx(LogLevel.Error, ex, true);
            }

            this._exception = ex;
        }

        #endregion Private Methods
    }
}