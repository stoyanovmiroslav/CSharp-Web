using Eventures.Data;
using Eventures.Models;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eventures.Services
{
    public class AdminService : IAdminService
    {
        private readonly EventuresDbContext db;
        private readonly UserManager<EventuresUser> userManager;

        public AdminService(EventuresDbContext db, UserManager<EventuresUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public void Demote(string id)
        {
            var user = this.db.Users.Include(x => x.UserRoles)
                            .FirstOrDefault(x => x.Id == id && x.UserRoles.Any(r => r.Role.Name == "Admin"));

            if (user == null)
            {
                return;
            }

            var userRole = user.UserRoles.FirstOrDefault(x => x.Role.Name == "Admin");

            if (userRole == null)
            {
                return;
            }

            this.db.UserRoles.Remove(userRole);
            this.db.SaveChanges();
        }

        public IList<EventuresUser> GetAllUsers()
        {
            var users = userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToList();

            return users;
        }

        public void Promote(string id)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Id == id && x.UserRoles.Any(r => r.Role.Name == "Admin") == false);

            if (user == null)
            {
                return;
            }

            var role = this.db.Roles.FirstOrDefault(x => x.Name == "Admin");

            if (role == null)
            {
                return;
            }

            var userRole = new EventuresUserRole
            {
                Role = role,
                UserId = user.Id
            };

            this.db.UserRoles.Add(userRole);
            this.db.SaveChanges();
        }
    }
}