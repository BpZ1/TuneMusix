using System;
using System.Text.RegularExpressions;

namespace TuneMusix.Helpers
{
    static class TextFormatService
    {
        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }
    }
}
