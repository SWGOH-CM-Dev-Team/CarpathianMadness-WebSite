
using System;
using System.Data.Common;
using System.Runtime.Serialization;


namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Standard Entity abstract class which is an extension
    /// of Basic Entity that represents a standardised record entity
    /// that has common fields like IsEnabled, IsDeleted, DateCreated, etc.
    /// </summary>
    [DataContract]
    public abstract class StandardEntity<TType> : BasicEntity<TType> where TType : StandardEntity<TType>, new()
    {
        #region Properties

        [DataMember]
        public bool IsDeleted
        {
            get;
            private set;
        }

        [DataMember]
        public DateTime? DateCreated
        {
            get;
            private set;
        }

        [DataMember]
        public DateTime? DateUpdated
        {
            get;
            private set;
        }

        [DataMember]
        public DateTime? DateDeleted
        {
            get;
            private set;
        }

        #endregion Properties

        #region Internal Methods

        internal override void InternalLoad(DbDataReader reader)
        {
            base.InternalLoad(reader);

            this.IsDeleted = (bool)reader["IsDeleted"];
            this.DateCreated = reader.GetValueOrDefault<DateTime?>("DateCreated");
            this.DateUpdated = reader.GetValueOrDefault<DateTime?>("DateUpdated");
            this.DateDeleted = reader.GetValueOrDefault<DateTime?>("DateDeleted");
        }

        internal override void InternalValidate(ErrorCollection errors)
        {
            base.InternalValidate(errors);

            if (this.IsDeleted)
            {
                errors.AddValue(1, "The object's record has been deleted...");
            }
        }

        #endregion Internal Methods

        #region Protected Methods

        protected abstract override void OnLoading(DbDataReader reader);

        protected abstract override void OnSaving(DatabaseContext context, CommandParameterCollection parameters, SaveType saveType);

        #endregion Protected Methods
    }
}
