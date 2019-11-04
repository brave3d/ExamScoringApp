using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamScoringApp.Helpers
{
    public static class StringExtensions
    {
            /// <summary>
            /// Get the string slice between the two indexes.
            /// Inclusive for start index, exclusive for end index.
            /// </summary>
        public static string Slice(this string source, int start, int end)
        {
            if (end < 0) // Keep this for negative end support
            {
                end = source.Length + end;
            }
            int len = end - start;               // Calculate length
            return source.Substring(start, len); // Return Substring of length
        }
        public static string Truncate(this string source, int length)
        {
            return length > source.Length ? source : source.Substring(0, length) + "...";
        }
    }
}