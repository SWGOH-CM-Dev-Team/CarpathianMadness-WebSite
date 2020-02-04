
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Struct representing an error, such as a validation error
    /// within an Entity when it's Validate() method is invoked.
    /// </summary>
    [DataContract]
    public sealed class Error
    {
        #region Properties

        /// <summary>
        /// Get the code of the error, if any.
        /// </summary>
        [DataMember(IsRequired = true)]
        public long Code
        {
            get;
            set;
        }

        /// <summary>
        /// Get the message describing the error.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Message
        {
            get;
            set;
        }

        #endregion Properties

        #region Public Methods

        public Error() : this(0, string.Empty)
        {

        }

        public Error(string message) : this(0, message)
        {

        }

        public Error(long code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        #endregion Public Methods
    }
}
