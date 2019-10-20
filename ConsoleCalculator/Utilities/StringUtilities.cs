using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCalculator.Utilities
{
    public static class StringUtilities
    {
        /// <summary>
        /// Filters out slashes for delimiter matching
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> FilterSlashes(string str)
        {
            return new List<string>() { str.Replace("//", string.Empty) };
        }

        /// <summary>
        /// Filters out slashes and splits brackets for delimiter matching
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> FilterSlashesAndSplitBrackets(string str)
        {
            return str.Replace("//", String.Empty).Replace("[", string.Empty).Split("]").ToList();
        }
    }
}
