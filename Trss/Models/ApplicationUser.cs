using System;
using AspNetCore.Identity.MongoDbCore.Models;

namespace Trss.Models
{
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName, string email) : base(userName, email)
        {
        }
    }

    public class ApplicationRole : MongoIdentityRole<Guid>
    {
        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName) : base(roleName)
        {
        }
    }
}
