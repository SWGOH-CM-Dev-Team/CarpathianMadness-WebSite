using System;
using System.Globalization;
using NLog;

namespace CarpathianMadness.Framework.NLog
{
    public static partial class NLogExtensions
    {
        #region Public Methods

        public static void TraceConditional(this Logger obj, string message)
        {
#if TRACE

            if ((obj == null) || string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            obj.Trace(message);

#endif
        }

        public static void DebugConditional(this Logger obj, string message)
        {
#if DEBUG

            if ((obj == null) || string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            obj.Debug(message);

#endif
        }

        public static void LogEx(this Logger obj, LogLevel level, Exception ex, bool includeStackTrace)
        {
            if ((obj == null) || (ex == null))
            {
                return;
            }

            if (!obj.IsEnabled(level))
            {
                return;
            }

            Exception e = ex;

            while (e != null)
            {
                obj.Log(level, string.Format(CultureInfo.InvariantCulture, "{0}: {1}", e.GetType().Name, e.Message));
                e = e.InnerException;
            }

            if (includeStackTrace)
            {
                obj.Log(level, ex.StackTrace);
            }
        }

        #endregion Public Methods
    }
}