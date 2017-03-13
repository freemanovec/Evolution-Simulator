using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace Evolution_Simulator.Computing
{
    static class Validation
    {
        /// <summary>
        /// Returns a valid absolute path with trailing backslash
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="modified">The string that is to be fixed or replaced with input string on success</param>
        public static void validateDirectoryPath(string input, out string modified)
        {
            string working = input;
            string regexAbsolute = @"^(\w:)(.)*$";
            bool isAbsolute = Regex.Match(working, regexAbsolute).Success;
            string regexEndsWithBackslash = @"^(.)*\\$";
            bool isTrailingBackslash = Regex.Match(working, regexEndsWithBackslash).Success;
            string regexStartsWithBackslash = @"^\\(.)*$";
            bool isLeadingBackslash = Regex.Match(working, regexStartsWithBackslash).Success;
            if (!isTrailingBackslash)
                working += @"\";
            if (!isAbsolute)
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                if (!isLeadingBackslash)
                    working = @"\" + working;
                working = currentDirectory + working;
            }
            modified = working;
        }
    }
}
