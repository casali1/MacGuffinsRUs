using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NewMacGuffinsRUs
{
    static class StringFuncs
    {
        // Split name into product number and other metadata
        public static string[] SplitName(string name)
        {
            return name.Split(new[] { '-' }, 2);
        }

        // Split metadata into an array of metadata pieces
        public static string[] SplitMetadata(string metadata)
        {
            // We use a regex to handle the case of X-size
            // The pattern is a negative lookbehind that matches - except when preceeded by x
            string pattern = "(?<![x])-";
            string[] splitData = Regex.Split(metadata, pattern, RegexOptions.IgnoreCase);
            return splitData;
        }
    }
}
