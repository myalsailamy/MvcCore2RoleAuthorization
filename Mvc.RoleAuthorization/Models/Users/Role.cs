using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.RoleAuthorization.Models.Users
{
    public class ApplicationRole : IdentityRole<int>
    {

        public ApplicationRole(){}
        public ApplicationRole(string roleName) : base(roleName){}
        public ApplicationRole(string roleName, string description) : base(roleName){ Description = description;}
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<IdentityRoleClaim<int>> Claims { get; set; }
    }
}
