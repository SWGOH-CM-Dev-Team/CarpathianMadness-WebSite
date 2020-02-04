
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace CarpathianMadness.Framework
{
    public static class CodeUtility
    {
        public static void Time(Action function)
        {
            Time(string.Empty, function);
        }

        public static void Time(string message, Action function)
        {
            var t0 = DateTime.UtcNow;
            if (function != null)
                function();
            var t1 = DateTime.UtcNow;
            var elapsed = (t1 - t0).TotalMilliseconds;
            var output = string.Empty;
            if (string.IsNullOrWhiteSpace(message))
                output = string.Format(CultureInfo.InvariantCulture, "CodeUtility.Time() took {0:F2}ms", elapsed);
            else
                output = string.Format(CultureInfo.InvariantCulture, "{0} took {1:F2}ms", message, elapsed);
            Debug.WriteLine(output);
        }


        #region Action Soaks

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ExecuteWithExceptionSoak(Action function)
        {
            try
            {
                if (function != null)
                    function();
            }
            catch
            {
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ExecuteWithExceptionSoak(Action function, Action onException)
        {
            try
            {
                if (function != null)
                    function();
            }
            catch
            {
                if (onException != null)
                    onException();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ExecuteWithExceptionSoak(Action function, Action<Exception> onException)
        {
            try
            {
                if (function != null)
                    function();
            }
            catch (Exception ex)
            {
                if (onException != null)
                    onException(ex);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ExecuteWithExceptionSoak<T1>(Action<T1> function, T1 p1, Action<Exception> onException = null)
        {
            try
            {
                if (function != null)
                    function(p1);
            }
            catch (Exception ex)
            {
                if (onException != null)
                    onException(ex);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ExecuteWithExceptionSoak<T1, T2>(Action<T1, T2> function, T1 p1, T2 p2, Action<Exception> onException = null)
        {
            try
            {
                if (function != null)
                    function(p1, p2);
            }
            catch (Exception ex)
            {
                if (onException != null)
                    onException(ex);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ExecuteWithExceptionSoak<T1, T2, T3>(Action<T1, T2, T3> function, T1 p1, T2 p2, T3 p3, Action<Exception> onException = null)
        {
            try
            {
                if (function != null)
                    function(p1, p2, p3);
            }
            catch (Exception ex)
            {
                if (onException != null)
                    onException(ex);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static void ExecuteWithExceptionSoak<T1, T2, T3, T4>(Action<T1, T2, T3, T4> function, T1 p1, T2 p2, T3 p3, T4 p4, Action<Exception> onException = null)
        {
            try
            {
                if (function != null)
                    function(p1, p2, p3, p4);
            }
            catch (Exception ex)
            {
                if (onException != null)
                    onException(ex);
            }
        }

        #endregion Action Soaks

        #region Func Soaks

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static TResult ExecuteWithExceptionSoak<TResult>(Func<TResult> function, Action<Exception> onException = null)
        {
            if (function == null)
            {
                return default(TResult);
            }

            try
            {
                return function();
            }
            catch (Exception ex)
            {
                if (onException != null)
                {
                    onException(ex);
                }
            }

            return default(TResult);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static TResult ExecuteWithExceptionSoak<TResult, T1>(Func<T1, TResult> function, T1 p1, Action<Exception> onException = null)
        {
            if (function == null)
            {
                return default(TResult);
            }

            try
            {
                return function(p1);
            }
            catch (Exception ex)
            {
                if (onException != null)
                {
                    onException(ex);
                }
            }

            return default(TResult);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static TResult ExecuteWithExceptionSoak<TResult, T1, T2>(Func<T1, T2, TResult> function, T1 p1, T2 p2, Action<Exception> onException = null)
        {
            if (function == null)
            {
                return default(TResult);
            }

            try
            {
                return function(p1, p2);
            }
            catch (Exception ex)
            {
                if (onException != null)
                {
                    onException(ex);
                }
            }

            return default(TResult);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static TResult ExecuteWithExceptionSoak<TResult, T1, T2, T3>(Func<T1, T2, T3, TResult> function, T1 p1, T2 p2, T3 p3, Action<Exception> onException = null)
        {
            if (function == null)
            {
                return default(TResult);
            }

            try
            {
                return function(p1, p2, p3);
            }
            catch (Exception ex)
            {
                if (onException != null)
                {
                    onException(ex);
                }
            }

            return default(TResult);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public static TResult ExecuteWithExceptionSoak<TResult, T1, T2, T3, T4>(Func<T1, T2, T3, T4, TResult> function, T1 p1, T2 p2, T3 p3, T4 p4, Action<Exception> onException = null)
        {
            if (function == null)
            {
                return default(TResult);
            }

            try
            {
                return function(p1, p2, p3, p4);
            }
            catch (Exception ex)
            {
                if (onException != null)
                {
                    onException(ex);
                }
            }

            return default(TResult);
        }

        #endregion Func Soaks
    }
}
