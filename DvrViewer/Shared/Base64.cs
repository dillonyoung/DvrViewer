using System;
using System.Text;

namespace DvrViewer.Shared
{
    /// <summary>
    /// Represents Base 64 functions
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// Encodes a string as a Base 64 string
        /// </summary>
        /// <param name="source">The source string to be encoded</param>
        /// <returns>Returns the encoded string</returns>
        public static string Encode(string source)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(source);

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Decodes a Base 64 encoded string
        /// </summary>
        /// <param name="source">The source string to be decoded</param>
        /// <returns>Returns the decoded string</returns>
        public static string Decode(string source)
        {
            byte[] bytes = Convert.FromBase64String(source);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
