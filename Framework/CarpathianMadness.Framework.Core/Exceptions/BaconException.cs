
using System;
using System.Runtime.Serialization;

namespace CarpathianMadness.Framework
{
    [Obsolete("Seriously, what are you actually going to use this for?")]
    [Serializable]
    public class BaconException : Exception
    {
        #region Constructors

        public BaconException(Exception innerException) : base("Bacon, Egg, Mushroom barm with brown sauce.", innerException)
        {

        }

        protected BaconException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        #endregion Constructors

        #region Public Methods

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion Public Methods
    }
}