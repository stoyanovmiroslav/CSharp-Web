using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Extensions
{
    public class StringExtensions
    {
        public string Capitalize(string value)
        {
            string capitalLetter = value.Substring(0, 1).ToUpper();
            string lowerLetters = value.Substring(1).ToLower();

            return $"{capitalLetter}{lowerLetters}";
        }
    }
}