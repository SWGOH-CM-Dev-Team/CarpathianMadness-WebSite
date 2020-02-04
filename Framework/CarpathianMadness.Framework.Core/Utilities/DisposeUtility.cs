
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpathianMadness.Framework
{
    /// <summary>
    /// Lazy utility static class for disposing objects
    /// that implement the IDisposable interface.
    /// </summary>
    public static class DisposeUtility
    {
        #region Public Methods

        public static void Dispose(params IDisposable[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                switch (objects.Length)
                {
                    case 1:
                        {
                            DisposeObject(objects[0]);
                            break;
                        }

                    case 2:
                        {
                            DisposeObject(objects[0]);
                            DisposeObject(objects[1]);
                            break;
                        }

                    case 3:
                        {
                            DisposeObject(objects[0]);
                            DisposeObject(objects[1]);
                            DisposeObject(objects[2]);
                            break;
                        }

                    case 4:
                        {
                            DisposeObject(objects[0]);
                            DisposeObject(objects[1]);
                            DisposeObject(objects[2]);
                            DisposeObject(objects[3]);
                            break;
                        }

                    default:
                        {
                            for (int i = 0; i < objects.Length; i++)
                            {
                                DisposeObject(objects[i]);
                            }

                            break;
                        }
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static void DisposeObject(IDisposable obj)
        {
            if (obj != null)
            {
                CodeUtility.ExecuteWithExceptionSoak(() => {
                    obj.Dispose();
                });
            }
        }

        #endregion Private Methods
    }
}
