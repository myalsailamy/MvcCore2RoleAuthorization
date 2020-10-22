using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mvc.RoleAuthorization.Data;
using Mvc.RoleAuthorization.Handlers;
using Mvc.RoleAuthorization.Models.Users;
using Mvc.RoleAuthorization.Services;

namespace Mvc.RoleAuthorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            IdentityBuilder identityBuilder = services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(ApplicationRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
            identityBuilder.AddRoleValidator<RoleValidator<ApplicationRole>>();
            identityBuilder.AddRoleManager<RoleManager<ApplicationRole>>();
            identityBuilder.AddSignInManager<SignInManager<ApplicationUser>>();
            identityBuilder.AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultUI();


            services.AddScoped<IDataAccessService, DataAccessService>();
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authorization", policyCorrectUser =>
                {
                    policyCorrectUser.Requirements.Add(new AuthorizationRequirement());
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                DbInitializer.Initialize(app);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });
            });
        }

        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //	if (env.IsDevelopment())
        //	{
        //		app.UseDeveloperExceptionPage();
        //		app.UseDatabaseErrorPage();

        //		DbInitializer.Initialize(app);
        //	}
        //	else
        //	{
        //		app.UseExceptionHandler("/Home/Error");
        //		app.UseHsts();
        //	}

        //	app.UseHttpsRedirection();
        //	app.UseStaticFiles();

        //	app.UseRouting();

        //	app.UseAuthentication();
        //	app.UseAuthorization();

        //	app.UseEndpoints(endpoints =>
        //	{
        //		endpoints.MapControllerRoute(
        //			name: "areaRoute",
        //			pattern: "{area:exists}/{controller}/{action}",
        //			defaults: new { action = "Index" });

        //		endpoints.MapControllerRoute(
        //			name: "default",
        //			pattern: "{controller=Home}/{action=Index}/{id?}");

        //		endpoints.MapRazorPages();
        //	});
        //}
    }
}