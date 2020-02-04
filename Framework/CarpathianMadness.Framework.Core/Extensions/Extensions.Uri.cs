
using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace CarpathianMadness.Framework
{
    public static class Extensions_Uri
    {
        #region Public Methods

        /// <summary>
        /// Gets a NameValueCollection instance of the query string, if any, from
        /// this Uri instance.
        /// </summary>
        public static NameValueCollection GetQueryString(this Uri uri)
        {
            return GetQueryString(uri, Encoding.UTF8);
        }

        /// <summary>
        /// Gets a NameValueCollection instance of the query string, if any, from
        /// this Uri instance.
        /// </summary>
        public static NameValueCollection GetQueryString(this Uri uri, Encoding encoding)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            NameValueCollection result;

            if (!string.IsNullOrWhiteSpace(uri.Query))
            {
                result = HttpUtility.ParseQueryString(uri.Query, encoding);
            }
            else
            {
                result = new NameValueCollection();
            }

            return result;
        }

        /// <summary>
        /// Get the root of the uri, e.g. http://www.example.com/
        /// </summary>
        public static Uri GetRoot(this Uri uri)
        {
            string absoluteUri = string.Empty;

            if (uri.IsAbsoluteUri)
            {
                absoluteUri = uri.AbsoluteUri;

                if (!string.IsNullOrEmpty(uri.PathAndQuery))
                {
                    absoluteUri = absoluteUri.Replace(uri.PathAndQuery, "/");
                }
            }

            return new Uri(absoluteUri);
        }

        /// <summary>
        /// Joins the provided relativeUri to this uri into a new instance.
        /// </summary>
        public static Uri Join(this Uri uri, string relativeUri)
        {
            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Uri must be an absolute uri.");
            }
            return new Uri(uri, relativeUri);
        }

        /// <summary>
        /// Joins the provided relativeUri to this uri into a new instance.
        /// </summary>
        public static Uri Join(this Uri uri, Uri relativeUri)
        {
            if (!uri.IsAbsoluteUri)
            {
                throw new ArgumentException("Uri must be an absolute uri.");
            }

            if (relativeUri.IsAbsoluteUri)
            {
                throw new ArgumentException("relativeUri must be a relative uri.");
            }

            return new Uri(uri, relativeUri);
        }

        /// <summary>
        /// Gets the path for this Uri instance depending if it is
        /// a relative or absolute uri.
        /// </summary>
        public static string GetFullPath(this Uri uri)
        {
            return uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.OriginalString;
        }

        #endregion Public Methods
    }
}
