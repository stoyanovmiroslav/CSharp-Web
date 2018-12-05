using System;
using System.Collections.Generic;
using System.Text;
using Eventures.Models;

namespace Eventures.Services.Contracts
{
    public interface IAdminService
    {
        IList<EventuresUser> GetAllUsers();

        void Promote(string id);

        void Demote(string id);
    }
}
