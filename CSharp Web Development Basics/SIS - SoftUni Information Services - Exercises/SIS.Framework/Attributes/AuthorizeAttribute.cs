using SIS.Framework.Security.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIS.Framework.Attributes
{
    public class AuthorizeAttribute : Attribute
    {
        private readonly string[] roles;

        public AuthorizeAttribute()
        {

        }

        public AuthorizeAttribute(params string[] roles)
        {
            this.roles = roles;
        }

        private bool IsIdentityPresent(IIdentity identity) =>
            identity != null;

        private bool IsIdentityInRole(IIdentity identity)
        {
            if (!this.IsIdentityPresent(identity))
            {
                return false;
            }

            var identityRoles = identity.Roles;

            foreach (var identityRole in identityRoles)
            {
                if (this.roles.Contains(identityRole))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsAuthenticated(IIdentity identity)
        {
            if (roles == null || !this.roles.Any())
            {
                return this.IsIdentityPresent(identity);
            }

            return this.IsIdentityInRole(identity);
        }
    }
}