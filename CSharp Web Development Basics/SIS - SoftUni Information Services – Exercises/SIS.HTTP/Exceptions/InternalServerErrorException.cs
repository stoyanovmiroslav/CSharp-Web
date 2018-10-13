using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public const string Message = "The Server has encountered an error.";

        public InternalServerErrorException()
            : base(Message)
        {
        }
    }
}
