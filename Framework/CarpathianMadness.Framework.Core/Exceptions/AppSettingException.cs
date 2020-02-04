
using System;
using System.Runtime.Serialization;

namespace CarpathianMadness.Framework
{
    [Serializable]
    public sealed class AppSettingException : Exception
    {
        public AppSettingException()
            : base()
        {
        }

        public AppSettingException(string message)
            : base(message)
        {
        }

        public AppSettingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private AppSettingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
