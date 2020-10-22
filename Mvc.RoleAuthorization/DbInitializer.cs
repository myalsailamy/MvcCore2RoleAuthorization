using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Mvc.RoleAuthorization.Data;
using Mvc.RoleAuthorization.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc.RoleAuthorization
{
	public static class DbInitializer
	{
		public static void Initialize(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
				context.Database.EnsureCreated();

				var _userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var _roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

				if (!context.Users.Any(usr => usr.UserName == "admin@test.com"))
				{
					var user = new ApplicationUser()
					{
						UserName = "admin@test.com",
						Email = "admin@test.com",
						EmailConfirmed = true,
					};

					var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
				}

				if (!context.Users.Any(usr => usr.UserName == "manager@test.com"))
				{
					var user = new ApplicationUser()
					{
						UserName = "manager@test.com",
						Email = "manager@test.com",
						EmailConfirmed = true,
					};

					var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
				}

				if (!context.Users.Any(usr => usr.UserName == "employee@test.com"))
				{
					var user = new ApplicationUser()
					{
						UserName = "employee@test.com",
						Email = "employee@test.com",
						EmailConfirmed = true,
					};

					var userResult = _userManager.CreateAsync(user, "P@ssw0rd").Result;
				}

				if (!_roleManager.RoleExistsAsync("Admin").Result)
				{
					var role = _roleManager.CreateAsync(new ApplicationRole { Name = "Admin" }).Result;
				}

				if (!_roleManager.RoleExistsAsync("Manager").Result)
				{
					var role = _roleManager.CreateAsync(new ApplicationRole { Name = "Manager" }).Result;
				}

				if (!_roleManager.RoleExistsAsync("Employee").Result)
				{
					var role = _roleManager.CreateAsync(new ApplicationRole { Name = "Employee" }).Result;
				}

				var adminUser = _userManager.FindByNameAsync("admin@test.com").Result;
				var adminRole = _userManager.AddToRolesAsync(adminUser, new string[] { "Admin" }).Result;

				var managerUser = _userManager.FindByNameAsync("manager@test.com").Result;
				var managerRole = _userManager.AddToRolesAsync(managerUser, new string[] { "Manager" }).Result;

				var employeeUser = _userManager.FindByNameAsync("employee@test.com").Result;
				var userRole = _userManager.AddToRolesAsync(employeeUser, new string[] { "Employee" }).Result;

                var permissions = GetPermissions();
                foreach (var item in permissions)
                {
                    if (!context.NavigationMenu.Any(n => n.Name == item.Name))
                    {
                        context.NavigationMenu.Add(item);
                    }
                }

                var _adminRole = _roleManager.Roles.Where(x => x.Name == "Admin").FirstOrDefault();
                var _managerRole = _roleManager.Roles.Where(x => x.Name == "Manager").FirstOrDefault();
                var _employeeRole = _roleManager.Roles.Where(x => x.Name == "Employee").FirstOrDefault();

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == 1))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = 1 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == 2))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = 2 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == 3))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = 3 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == 4))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = 4 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == 5))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = 5 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == 6))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = 6 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _adminRole.Id && x.NavigationMenuId == 7))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _adminRole.Id, NavigationMenuId = 7 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _managerRole.Id && x.NavigationMenuId == 1))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _managerRole.Id, NavigationMenuId = 1 });
                }

                if (!context.RoleMenuPermission.Any(x => x.RoleId == _managerRole.Id && x.NavigationMenuId == 2))
                {
                    context.RoleMenuPermission.Add(new RoleMenuPermission() { RoleId = _managerRole.Id, NavigationMenuId = 2 });
                }

                context.SaveChanges();
			}
		}

        private static List<NavigationMenu> GetPermissions()
        {
            return new List<NavigationMenu>()
            {
                new NavigationMenu()
                {
                    Name = "Admin",
                    ControllerName = "",
                    ActionName = "",
                    ParentMenuId = null,
                    DisplayOrder=1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Name = "Roles",
                    ControllerName = "Admin",
                    ActionName = "Roles",
                    ParentMenuId = 1,
                    DisplayOrder=1,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Name = "Users",
                    ControllerName = "Admin",
                    ActionName = "Users",
                    ParentMenuId = 1,
                    DisplayOrder=3,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Name = "External Google Link",
                    ControllerName = "",
                    ActionName = "",
                    IsExternal = true,
                    ExternalUrl = "https://www.google.com/",
                    ParentMenuId = 1,
                    DisplayOrder=2,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Name = "Create Role",
                    ControllerName = "Admin",
                    ActionName = "CreateRole",
                    ParentMenuId = 1,
                    DisplayOrder=3,
                    Visible = true,
                },
                new NavigationMenu()
                {
                    Name = "Edit User",
                    ControllerName = "Admin",
                    ActionName = "EditUser",
                    ParentMenuId = 1,
                    DisplayOrder=3,
                    Visible = false,
                },
                new NavigationMenu()
                {
                    Name = "Edit Role Permission",
                    ControllerName = "Admin",
                    ActionName = "EditRolePermission",
                    ParentMenuId = 1,
                    DisplayOrder=3,
                    Visible = false,
                },
            };
        }
    }
}