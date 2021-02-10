// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringIndexOfIsCultureSpecific.1

using System;
using System.Collections.Generic;
using System.Linq;

namespace levelled.utils
{
    /// <summary>
    /// Add additional methods to string type, which simply process of strings to other types
    /// or ease string manipulation
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Return list of items where each is trimmed and all blanks are removed
        /// </summary>
        /// <param name="list">Input list</param>
        /// <returns>List of items which has no blanks and elements have no leading or trailing white space</returns>
        public static IEnumerable<string> TrimAllNoBlanks(this IEnumerable<string> list)
        {
            return TrimAll(list).Where(s => s.Length > 0);
        }

        public static IEnumerable<string> AllUppercase(this IEnumerable<string> list)
        {
            return list.Select(s => s.ToUpper());
        }


        /// <summary>
        /// Return list of items where each is trimmed
        /// </summary>
        /// <param name="list">Input list</param>
        /// <returns>List of items where elements have no leading or trailing white space</returns>
        public static IEnumerable<string> TrimAll(IEnumerable<string> list)
        {
            return list.Select(s => s.Trim());
        }

        /// <summary>
        /// Search input string to find position of search string
        /// </summary>
        /// <param name="input">Input string to scan for search text</param>
        /// <param name="find">String to search for</param>
        /// <param name="start">Start position of search</param>
        /// <returns>Position of search string in input string</returns>
        public static int PositionOf(this string input, string find, int start = 0)
        {
            return input.IndexOf(find, start, StringComparison.Ordinal);
        }

        /// <summary>
        /// Compare two strings and ignore case and trailing space
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="match">String to match</param>
        /// <returns>True if strings match</returns>
        public static bool Matches(this string input, string match)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return input.Trim().ToLower() == match.Trim().ToLower();
        }

        /// <summary>
        /// Return true if match string occurs in input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="match">String to match</param>
        /// <returns>True if match string found in input string</returns>
        public static bool Comprises(this string input, string match)
        {
            var matches = input.Trim().ToLower().Contains(match.Trim().ToLower());
            return matches;
        }

        /// <summary>
        /// Search input string to find position of given character
        /// </summary>
        /// <param name="input">Input string to scan for search text</param>
        /// <param name="find">Character to search for</param>
        /// <param name="start">Start position of search</param>
        /// <returns>Position of specified character in input string</returns>
        public static int PositionOf(this string input, char find, int start = 0)
        {
            return input.IndexOf(find, start);
        }

        /// <summary>
        /// Split string into a list, where the specified character indicates the separator
        /// </summary>
        /// <param name="input">Input string to be split</param>
        /// <param name="separator">Character that denotes each split point</param>
        /// <returns>List of strings</returns>
        public static List<string> ToList(this string input, char separator = '\n')
        {
            return input.Split(separator).TrimAllNoBlanks().ToList();
        }

        /// <summary>
        /// Split string into list and convert to an Array type
        /// </summary>
        /// <param name="input">Input string to be converted into Array</param>
        /// <param name="separator">Character that denotes each split point</param>
        /// <returns>Array of strings</returns>
        public static string[] ToArray(this string input, char separator = '\n')
        {
            // ReSharper disable once RemoveToList.1
            return input.ToList(separator).ToArray();
        }

        /// <summary>
        /// Extract text between two specified markers (e.g. {{ }})
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="startMarker">Token marking start of text to be extracted</param>
        /// <param name="endMarker">Token marking end of text to be extracted</param>
        /// <returns>Extracted text, which does not contain the markers</returns>
        public static string ExtractTextBetween(this string input, string startMarker, string endMarker)
        {
            var start = input.PositionOf(startMarker);

            if (start == -1)
                return null;

            if (input.IndexOf(startMarker) < 0)
                throw new Exception($"startMarker:{startMarker} not found in input:{input}");

            if (input.IndexOf(endMarker) < 0)
                throw new Exception($"endMarker:{endMarker} not found in input:{input}");

            var end = input.PositionOf(endMarker, start + 1);
            var length = (end - start) - endMarker.Length;

            string token = "";

            if (length > 0)
                token = input.Substring(start + startMarker.Length, length);

            return token;
        }

        /// <summary>
        /// Split a string and take an optional segment
        /// </summary>
        /// <param name="input">Input text</param>
        /// <param name="segment">Number of segment to take</param>
        /// <param name="separator">Separator character used as split points</param>
        /// <returns>Requested segment or null if none available</returns>
        public static string Segment(this string input, int segment, char separator = '/')
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (separator == ' ')
                input = input.Replace('\t', ' ');

            return input.Split(separator).Skip(segment).Take(1).FirstOrDefault() ?? "";
        }

        /// <summary>
        /// Cleanup whitespace in strings by replacing tabs with space and reducing double spaces to single space
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Cleaned string</returns>
        public static string CleanWhitespace(this string input)
        {
            input = input.Replace('\t', ' ');

            while (input.IndexOf("  ") > -1)
                input = input.Replace("  ", " ");

            return input.Trim();
        }

        /// <summary>
        /// Convert string to lowercase and strip trailing space
        /// </summary>
        /// <param name="input">Input text</param>
        /// <returns>Clean string</returns>
        public static string Clean(this string input)
        {
            return input.Trim().ToLower();
        }

        /// <summary>
        /// Extract an enum value from the text value
        /// </summary>
        /// <typeparam name="EType">Enumeration type</typeparam>
        /// <param name="input">String value</param>
        /// <returns></returns>
        public static EType ToEnum<EType>(this string input)
        where EType : Enum
        {
            Enum.TryParse(typeof(EType), input, true, out var value);
            return (EType)value;
        }

        /// <summary>
        /// Extract a bool value from the text value
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns>Boolean value</returns>
        public static bool ToBool(this string input)
        {
            _ = bool.TryParse(input, out var val);
            return val;
        }

        /// <summary>
        /// Extract an integer value from the text value
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns>Integer value</returns>
        public static int ToInt(this string input)
        {
            _ = int.TryParse(input, out var val);
            return val;
        }

        /// <summary>
        /// Extract a floating point number from the text value
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns>Float value</returns>
        public static float ToFloat(this string input)
        {
            _ = float.TryParse(input, out var val);
            return val;
        }

        /// <summary>
        /// Wrap input string in quotes
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns></returns>
        public static string DoubleQuote(this string input)
        {
            return "\"" + (input ?? "<null>").Trim() + "\"";
        }

        /// <summary>
        /// Wrap input value in single quotes
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns></returns>
        public static string SingleQuote(this string input)
        {
            return "'" + (input ?? "<null>").Trim().UseSingleQuotes() + "'";
        }

        /// <summary>
        /// Remove quotes from string
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns></returns>
        public static string Unquote(this string input)
        {
            return input.Trim().Trim('\"');
        }

        /// <summary>
        /// Replace double quotes with single quotes
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns></returns>
        public static string UseSingleQuotes(this string input)
        {
            return (input ?? "<null>").Replace("\"", "'");
        }
    }
}
