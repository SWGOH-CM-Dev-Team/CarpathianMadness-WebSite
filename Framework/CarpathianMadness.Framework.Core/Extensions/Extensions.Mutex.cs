
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        #region Public Methods

        public static bool WaitAndExecute(this Mutex instance, int millisecondTimeout, Action action, out Exception caughtException)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (millisecondTimeout < 0)
            {
                throw new ArgumentOutOfRangeException("millisecond");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            bool hasHandle = false;
            caughtException = null;

            try
            {
                if (!instance.WaitOne(millisecondTimeout > 0 ? millisecondTimeout : Timeout.Infinite))
                {
                    return false;
                }

                hasHandle = true;
                action();

                return true;
            }
            catch (Exception ex)
            {
                caughtException = ex;
                return false;
            }
            finally
            {
                if (hasHandle)
                {
                    instance.ReleaseMutex();
                }
            }
        }

        public static bool WaitAndExecute(this Mutex instance, int millisecondTimeout, Action action)
        {
            Exception ex;
            return WaitAndExecute(instance, millisecondTimeout, action, out ex);
        }

        public static bool WaitAndExecute(this Mutex instance, Action action)
        {
            Exception ex;
            return WaitAndExecute(instance, Timeout.Infinite, action, out ex);
        }

        public static bool WaitAndExecute(this Mutex instance, Action action, out Exception caughtException)
        {
            return WaitAndExecute(instance, Timeout.Infinite, action, out caughtException);
        }

        #endregion Public Methods
    }
}