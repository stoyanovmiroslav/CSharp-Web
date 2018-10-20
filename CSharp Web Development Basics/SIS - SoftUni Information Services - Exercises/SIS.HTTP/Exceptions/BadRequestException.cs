using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string MESSAGE = "The Request was malformed or contains unsupported elements.";

        public BadRequestException() 
            : base(MESSAGE)
        {
        }

        public BadRequestException(string massage)
          : base(massage)
        {
        }
    }
}
