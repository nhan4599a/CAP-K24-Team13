using System;

namespace AuthServer.Extensions
{
    public static class StringExtension
    {
        public static string ToBase64(this string plainText)
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(plainText));
        }

        public static string FromBase64(string baseEncodedText)
        {
            return System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(baseEncodedText));
        }
    }
}
