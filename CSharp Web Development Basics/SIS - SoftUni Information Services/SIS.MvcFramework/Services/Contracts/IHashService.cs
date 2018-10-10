using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Services.Contracts
{
    public interface IHashService
    {
        string Hash(string stringToHash);

        string StrongHash(string stringToHash);
    }
}