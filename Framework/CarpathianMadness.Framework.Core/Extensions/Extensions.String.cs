
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace CarpathianMadness.Framework
{
    public static partial class Extensions
    {
        #region Public Methods

        /// <summary>
        /// Checks if the string's length within the specified minimum and maximum length.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static bool IsBetween(this string value, int minimumLength, int maximumLength)
        {
            if (value == null)
                throw new NullReferenceException();
            return (value.Length >= minimumLength) && (value.Length <= maximumLength);
        }

        /// <summary>
        /// Creates a repeating string.
        /// </summary>
        /// <param name="count">The number of times to repeat the string.</param>
        /// <returns>A string containing 'count' number of repetitions of the original string.</returns>
        public static string Repeat(this string value, int count)
        {
            const int MaxCount = 1000;
            string result = string.Empty;
            if (!string.IsNullOrEmpty(value) && (count > 0) && (count <= MaxCount))
                for (int i = 0; i < count; i++)
                    result += value;
            return result;
        }

        public static string ExtractBetween(this string value, string start, string end)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (string.IsNullOrEmpty(start))
                return string.Empty;
            string result = string.Empty;

            int i = value.IndexOf(start, StringComparison.Ordinal);
            if (i >= 0)
            {
                i += start.Length;
                if (string.IsNullOrEmpty(end))
                    return value.Substring(i);

                int j = value.IndexOf(end, i + 1, StringComparison.Ordinal);
                if (j >= 0)
                {
                    return value.Substring(i, j - i);
                }
                else
                {
                    return value.Substring(i);
                }

            }
            return result;
        }

        public static short[] SplitToShort(this string[] values)
        {
            var results = new List<short>();
            foreach (var item in values)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var temp = (short)0;
                    if (short.TryParse(item, NumberStyles.Integer, CultureInfo.InvariantCulture, out temp))
                        results.Add(temp);
                }
            }
            return results.ToArray();
        }

        public static int[] SplitToInt(this string[] values)
        {
            var results = new List<int>();
            foreach (var item in values)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var temp = (int)0;
                    if (int.TryParse(item, NumberStyles.Integer, CultureInfo.InvariantCulture, out temp))
                        results.Add(temp);
                }
            }
            return results.ToArray();
        }

        public static long[] SplitToLong(this string[] values)
        {
            var results = new List<long>();
            foreach (var item in values)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var temp = (long)0;
                    if (long.TryParse(item, NumberStyles.Integer, CultureInfo.InvariantCulture, out temp))
                        results.Add(temp);
                }
            }
            return results.ToArray();
        }

        public static string Left(this string value, string end)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (string.IsNullOrEmpty(end))
                return value;

            int i = value.IndexOf(end, StringComparison.Ordinal);
            if (i >= 0)
            {
                return value.Substring(0, i);
            }
            return value;
        }

        public static string Right(this string value, string begin)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (string.IsNullOrEmpty(begin))
                return value;

            int i = value.IndexOf(begin, StringComparison.Ordinal);
            if (i >= 0)
            {
                return value.Substring(i + 1);
            }
            return value;
        }

        public static IXPathNavigable ToXml(this string value)
        {
            var result = new XmlDocument();
            try
            {
                result.LoadXml(value.Trim());
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static XmlDocument ToXmlDocument(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var document = new XmlDocument();

            try
            {
                document.LoadXml(value);
            }
            catch
            {
                return null;
            }

            return document;
        }

        public static IDictionary<string, string> XmlFragmentToDictionary(this string value)
        {
            var results = new Dictionary<string, string>();
            var name = string.Empty;
            var text = string.Empty;

            using (var tr = new XmlTextReader(value, XmlNodeType.Element, null))
            {
                while (tr.Read())
                {
                    switch (tr.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (!string.IsNullOrWhiteSpace(name))
                                results.Add(name, text); // store the accumulated pair...
                            name = tr.Name;
                            text = string.Empty;
                            break;
                        case XmlNodeType.Text:
                            text = tr.Value;
                            break;
                    }
                }
                tr.Close();
            }
            if (!string.IsNullOrWhiteSpace(name))
                results.Add(name, text); // store the final accumulated pair...

            return results;
        }

        public static string Mask(this string value)
        {
            return Mask(value, '*', 4);
        }

        public static string Mask(this string value, char mask, int lengthToNotMask)
        {
            string result = null;

            if (!string.IsNullOrWhiteSpace(value) && value.Length >= lengthToNotMask)
            {
                int difference = value.Length - lengthToNotMask;
                result = new string(mask, difference) + value.Substring(difference, lengthToNotMask);
            }

            return result;
        }

        public static string MaskAll(this string value)
        {
            return Mask(value, '*', 0);
        }

        public static string MaskAll(this string value, char mask)
        {
            return Mask(value, mask, 0);
        }

        public static bool ToBoolean(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                switch (value.ToUpperInvariant())
                {
                    case "TRUE":
                    case "T":
                    case "YES":
                    case "Y":
                    case "ON":
                    case "1":
                    case "-1":
                        return true;
                }
            }
            return false;
        }

        public static string RemoveConsecutiveDuplicates(this string obj, char character)
        {
            var result = obj;
            var replace = character.ToString();
            var search = replace + replace;
            while (result.Contains(search))
                result = result.Replace(search, replace);
            return result;
        }

        public static string RemoveNonAllowed(this string obj, string allowable)
        {
            if (string.IsNullOrWhiteSpace(allowable))
                return string.Empty;
            return RemoveNonAllowed(obj, allowable.ToCharArray(), null);
        }

        public static string RemoveNonAllowed(this string obj, string allowable, char replace)
        {
            if (string.IsNullOrWhiteSpace(allowable))
                return string.Empty;
            return RemoveNonAllowed(obj, allowable.ToCharArray(), replace);
        }

        private static string RemoveNonAllowed(this string obj, char[] allowable, char? replace)
        {
            if ((allowable == null) || (allowable.Length == 0))
                return string.Empty;

            var allow = allowable.Distinct();
            var result = new StringBuilder();

            foreach (var character in obj.ToCharArray())
                if (allow.Contains(character))
                    result.Append(character);
                else
                    if (replace.HasValue)
                    result.Append(replace.Value);

            return result.ToString();
        }


        private const string InitialisationSalt = "dj75hy43ty78mw56;";
        private const int KeySize = 256;

        public static string Encrypt(this string obj, string passPhrase)
        {
            var result = (byte[])null;

            using (var password = new PasswordDeriveBytes(passPhrase, null))
            {
                using (var symmetricKey = new RijndaelManaged())
                {

                    symmetricKey.Mode = CipherMode.CBC;
                    var keyBytes = password.GetBytes(KeySize / 8);
                    var initVectorBytes = Encoding.UTF8.GetBytes(InitialisationSalt);

                    using (var memoryStream = new MemoryStream())
                    {
                        var encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            var plainTextBytes = Encoding.UTF8.GetBytes(obj);
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cryptoStream.Close();
                        }
                        result = memoryStream.ToArray();
                        memoryStream.Close();
                    }
                }
            }
            return Convert.ToBase64String(result);
        }

        public static string Decrypt(this string obj, string passPhrase)
        {
            var result = string.Empty;

            var password = (PasswordDeriveBytes)null;
            try
            {
                password = new PasswordDeriveBytes(passPhrase, null);

                var symmetricKey = (RijndaelManaged)null;
                try
                {
                    symmetricKey = new RijndaelManaged();
                    //{ Mode = CipherMode.CBC };
                    symmetricKey.Mode = CipherMode.CBC;
                    var cipherTextBytes = Convert.FromBase64String(obj);

                    var memoryStream = (MemoryStream)null;
                    try
                    {
                        memoryStream = new MemoryStream(cipherTextBytes);

                        var initVectorBytes = Encoding.ASCII.GetBytes(InitialisationSalt);
                        var keyBytes = password.GetBytes(KeySize / 8);
                        var decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                        var plainTextBytes = (byte[])null;
                        var decryptedByteCount = (int)0;

                        var cryptoStream = (CryptoStream)null;
                        try
                        {
                            cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                            plainTextBytes = new byte[cipherTextBytes.Length];
                            decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.Close();
                        }
                        finally
                        {
                            if (cryptoStream != null)
                                cryptoStream.Dispose();
                        }
                        memoryStream.Close();
                        result = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                    }
                    finally
                    {
                        if (memoryStream != null)
                            memoryStream.Dispose();
                    }
                }
                finally
                {
                    if (symmetricKey != null)
                        symmetricKey.Dispose();
                }
            }
            finally
            {
                if (password != null)
                    password.Dispose();
            }
            return result;
        }

        public static string First(this string value, int count)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            int getLength = value.Length < count ? value.Length : count;

            if (getLength == 0)
            {
                return string.Empty;
            }

            return value.Substring(0, getLength);
        }

        public static string Last(this string value, int count)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            int startPosition = value.Length - count;

            if (startPosition < 0)
            {
                return string.Empty;
            }

            return value.Substring(startPosition);
        }

        public static string AppendWithDelimiter(this string obj, string append, string delimiter)
        {
            if ((obj == null) && (append == null))
                return null;

            var result = obj ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(append))
            {
                if (!string.IsNullOrWhiteSpace(result))
                    result += (delimiter ?? string.Empty);
                result += append;
            }
            return result;
        }


        /// <summary>
        /// Restores a string converted to base64
        /// </summary>
        /// <param name="s"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public static string RestoreFromBase64(this string s, Action callBack)
        {
#if DEBUG
            // Used to bypass string encoding if text is wrapped with "
            var mtch = System.Text.RegularExpressions.Regex.Match(s, "^\"(?<Text>.+)\"$", System.Text.RegularExpressions.RegexOptions.Compiled);

            if (mtch.Groups["Text"].Success)
                return mtch.Groups["Text"].Value;
#endif
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(s));
            }
            catch (Exception ex)
            {
                if (callBack != null)
                    callBack();
                else
                    throw new Exception("Unable to restore string.", ex);
            }
            return null;
        }

        public static Byte[] ToBytes(this string instance)
        {
            return ToBytes(instance, Encoding.UTF8);
        }

        public static Byte[] ToBytes(this string instance, Encoding encoding)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return encoding.GetBytes(instance);
        }

        public static string ToBase64(this string instance)
        {
            return ToBase64(instance, Encoding.UTF8);
        }

        public static string ToBase64(this string instance, Encoding encoding)
        {
            if (instance == null)
            {
                return null;
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            byte[] instanceBytes = encoding.GetBytes(instance);

            try
            {
                return Convert.ToBase64String(instanceBytes);
            }
            catch
            {
                throw;
            }
        }

        public static string FromBase64(this string instance)
        {
            return FromBase64(instance, Encoding.UTF8);
        }

        public static string FromBase64(this string instance, Encoding encoding)
        {
            if (instance == null)
            {
                return null;
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            try
            {
                byte[] decodedBytes = Convert.FromBase64String(instance);
                return encoding.GetString(decodedBytes);
            }
            catch
            {
                throw;
            }
        }

        public static string ToSha256(this string instance)
        {
            return ToSha256(instance, Encoding.UTF8);
        }

        public static string ToSha256(this string instance, Encoding encoding)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            using (var hasher = new SHA256Managed())
            {
                return instance.ToHash(hasher, encoding);
            }
        }

        public static string ToMd5(this string instance)
        {
            return ToMd5(instance, Encoding.UTF8);
        }

        public static string ToMd5(this string instance, Encoding encoding)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            using (var hasher = new MD5CryptoServiceProvider())
            {
                return instance.ToHash(hasher, encoding);
            }
        }

        public static string ToHash(this string instance, HashAlgorithm hasher)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.ToHash(hasher, Encoding.UTF8);
        }

        public static string ToHash(this string instance, HashAlgorithm hasher, Encoding encoding)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            byte[] instanceBytes = instance.ToBytes(encoding);
            byte[] hashedBytes = hasher.ComputeHash(instanceBytes);

            return hashedBytes.ToHexadecimalString(false);
        }

        public static string ToHmacSha256(this string instance, string key)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            Encoding encoding = Encoding.UTF8;

            using (var hasher = new HMACSHA256(key.ToBytes(encoding)))
            {
                return instance.ToHmac(hasher, encoding);
            }
        }

        public static string ToHmacSha256(this string instance, string key, Encoding encoding)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            using (var hasher = new HMACSHA256(key.ToBytes(encoding)))
            {
                return instance.ToHmac(hasher, encoding);
            }
        }

        public static string ToHmac(this string instance, HashAlgorithm hasher)
        {
            return ToHmac(instance, hasher, Encoding.UTF8);
        }

        public static string ToHmac(this string instance, HashAlgorithm hasher, Encoding encoding)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (hasher == null)
            {
                throw new ArgumentNullException("hasher");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            byte[] instanceBytes = instance.ToBytes(encoding);
            byte[] hashedBytes = hasher.ComputeHash(instanceBytes);

            return hashedBytes.ToBase64();
        }

        public static bool EqualsAnyInvariantIgnoreCase(this string instance, params string[] values)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length == 0)
            {
                throw new ArgumentOutOfRangeException("values");
            }

            foreach (string value in values)
            {
                if (instance.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool EqualsAny(this string instance, StringComparison comparer, params string[] values)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length == 0)
            {
                throw new ArgumentOutOfRangeException("values");
            }

            foreach (string value in values)
            {
                if (instance.Equals(value, comparer))
                {
                    return true;
                }
            }

            return false;
        }

        public static string Replace(this string instance, string oldValue, string newValue, StringComparison comparer)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            if (oldValue == null)
            {
                throw new ArgumentNullException("oldValue");
            }

            if (newValue == null)
            {
                throw new ArgumentNullException("newValue");
            }

            int oldValueLength = oldValue.Length;
            string result = instance;
            int lastIndex = 0;

            while ((lastIndex = result.IndexOf(oldValue, lastIndex, comparer)) > -1)
            {
                result = result.Remove(lastIndex, oldValueLength).Insert(lastIndex, newValue);
            }

            return result;
        }

        #endregion Public Methods
    }
}
