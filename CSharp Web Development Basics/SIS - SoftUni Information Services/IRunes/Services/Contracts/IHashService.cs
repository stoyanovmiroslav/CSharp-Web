using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Contracts
{
    public interface IHashService
    {
        string Hash(string stringToHash);

        string StrongHash(string stringToHash);
    }
}