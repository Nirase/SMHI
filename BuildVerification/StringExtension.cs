using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildVerification
{
    static class StringExtension
    {
        /// <summary>
        /// Shortens strings to remove unnecessary information
        /// </summary>
        /// <param name="value">String to adjust</param>
        /// <param name="maxLength">New max length for string</param>
        /// <returns>The new string</returns>
        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
