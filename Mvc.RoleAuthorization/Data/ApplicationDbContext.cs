using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mvc.RoleAuthorization.Models.Users;

namespace Mvc.RoleAuthorization.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<RoleMenuPermission> RoleMenuPermission { get; set; }
        public DbSet<NavigationMenu> NavigationMenu { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<RoleMenuPermission>().HasKey(c => new { c.RoleId, c.NavigationMenuId });
            base.OnModelCreating(builder);
        }
    }
}