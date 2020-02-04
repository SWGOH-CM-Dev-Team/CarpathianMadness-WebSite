
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using CarpathianMadness.Framework;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Basic Entity abstract class which if implemented will
    /// treat the inherited class as a read/write record suitable
    /// for representing data from tables.
    /// </summary>
    [DataContract]
    public abstract class BasicEntity<TType> : Entity<TType> where TType : BasicEntity<TType>, new()
    {
        #region Members

        private bool _isNew = true;

        #endregion Members

        #region Properties

        /// <summary>
        /// Get if this object is new and hasn't been saved yet or has been loaded
        /// from a query.
        /// </summary>
        [IgnoreDataMember]
        public bool IsNew
        {
            get
            {
                return this._isNew;
            }

            internal set
            {
                this._isNew = value;
            }
        }

        #endregion Properties

        #region Internal Methods

        internal override void InternalLoad(DbDataReader reader)
        {
            base.InternalLoad(reader);

            this.IsNew = false;
        }

        internal virtual void InternalValidate(ErrorCollection errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException("errors");
            }

            OnValidate(errors);
        }

        internal virtual void InternalSave(DatabaseContext context, CommandParameterCollection parameters, SaveType saveType)
        {
            OnSaving(context, parameters, saveType);
        }

        internal virtual void InternalDelete(CommandParameterCollection parameters)
        {
            OnDeleting(parameters);
        }

        #endregion Internal Methods

        #region Public Methods

        public ErrorCollection Validate()
        {
            ErrorCollection errors = new ErrorCollection();
            InternalValidate(errors);

            return errors;
        }

        public void Validate(ErrorCollection errors)
        {
            InternalValidate(errors);
        }

        #endregion Public Methods

        #region Protected Methods

        protected abstract override void OnLoading(DbDataReader reader);

        protected virtual void OnValidate(ErrorCollection errors)
        {
            //Do nothing.
        }

        protected abstract void OnSaving(DatabaseContext context, CommandParameterCollection parameters, SaveType saveType);

        protected virtual void OnDeleting(CommandParameterCollection parameters)
        {
            //Do nothing.
        }

        #endregion Protected Methods
    }
}