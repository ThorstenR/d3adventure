using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.TextProcessing
{
    public class TP
    {
        public static string HexAsciiConvert(string hex)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= hex.Length - 2; i += 2)
            {

                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2),

                System.Globalization.NumberStyles.HexNumber))));

            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the text between two delimiters
        /// Isolate("<p>hello world</p>", "<p>", "</p>")
        /// returns "hello world"
        /// </summary>
        public static string Isolate(string source, string start, string end)
        {
            int len1 = start.Length;
            int len2 = end.Length;

            int loc1 = source.IndexOf(start);
            int loc2 = source.IndexOf(end);

            int startLoc = loc1 + len1;
            int endLoc = loc2 - startLoc;

            return source.Substring(startLoc, endLoc);
        }

        /// <summary>
        /// Removes a string between two delimiters. Opposite of Isolate
        /// RemoveString("<p>hello world</p>", "<p>", "</p>")
        /// returns "<p></p>"
        /// </summary>
        public static string RemoveString(string source, string start, string end)
        {
            int len1 = start.Length;
            int len2 = end.Length;

            int loc1 = source.IndexOf(start);
            int loc2 = source.IndexOf(end);

            int startLoc = loc1;
            int endLoc = loc2 + len2 - startLoc;

            return source.Remove(startLoc, endLoc);
        }
        
        /// <summary>
        /// Can split a string such as str = "<p>hello</p> <p>world</p>"
        /// IsolateSplit(str, "<p>", "</p>")
        /// returns an array with ret[0]="hello" ret[1]="world"
        /// </summary>
        public static string[] IsolateSplit(string source, string start, string end)
        {
            int len1 = start.Length;
            int len2 = end.Length;

            int loc1 = source.IndexOf(start);
            int loc2 = source.IndexOf(end);

            int startLoc = loc1 + len1;
            int endLoc = loc2 - startLoc;

            List<string> parts = new List<string>();

            while (loc1 > -1 && loc2 > -1)
            {
                parts.Add(source.Substring(startLoc, endLoc));
                source = source.Substring(loc2 + len2);

                loc1 = source.IndexOf(start);
                loc2 = source.IndexOf(end);
            }

            string[] partsReturn = new string[parts.Count];
            return partsReturn;
        }


        #region ByteTools
        /// <summary>
        /// Verifies if char is base 16
        /// </summary>
        public bool TestHexChar(char hexChar)
        {
            int i = (int)hexChar;
            if (i >= 40 && i <= 57
                || i >= 65 && i <= 70
                || i >= 97 && i <= 102)
                return true;
            else return false;
        }

        /// Thanks to Tomalak
        /// http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa-in-c
        /// Modified by CTS_AE
        public byte[] HexStringToByteArray(String hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new Exception("HexStringToByteArray hex parameter must contain an even ammount of characters");
            }

            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                if (TestHexChar(hex.Substring(i, 1).ToCharArray()[0]) && TestHexChar(hex.Substring(i + 1, 1).ToCharArray()[0]))
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                else throw new Exception("HexStringToByteArray hex parameter must contain base16/hex characters");
            return bytes;
        }

        /// Thanks to ScottyDoesKnow
        /// http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/15514c1a-b6a1-44f5-a06c-9b029c4164d7
        /// Modified by CTS_AE
        #region ByteSearching
        private class PartialMatch
        {
            public int Index { get; private set; }
            public int MatchLength { get; set; }

            public PartialMatch(int index)
            {
                Index = index;
                MatchLength = 1;
            }
        }

        private static int ByteIndexOf(byte[] arrayToSearch, byte[] patternToFind)
        {
            if (patternToFind.Length == 0
              || arrayToSearch.Length == 0
              || arrayToSearch.Length < patternToFind.Length)
                return -1;

            List<PartialMatch> partialMatches = new List<PartialMatch>();

            for (int i = 0; i < arrayToSearch.Length; i++)
            {
                for (int j = partialMatches.Count - 1; j >= 0; j--)
                    if (arrayToSearch[i] == patternToFind[partialMatches[j].MatchLength])
                    {
                        partialMatches[j].MatchLength++;

                        if (partialMatches[j].MatchLength == patternToFind.Length)
                            return partialMatches[j].Index;
                    }
                    else
                        partialMatches.Remove(partialMatches[j]);

                if (arrayToSearch[i] == patternToFind[0])
                {
                    if (patternToFind.Length == 1)
                        return i;
                    else
                        partialMatches.Add(new PartialMatch(i));
                }
            }

            return -1;
        }
        #endregion
        #endregion

    }
}
