using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string value)
        {
            string capitalLetter = value.Substring(0, 1).ToUpper();
            string lowerLetters = value.Substring(1).ToLower();

            return $"{capitalLetter}{lowerLetters}";
        }
    }
}