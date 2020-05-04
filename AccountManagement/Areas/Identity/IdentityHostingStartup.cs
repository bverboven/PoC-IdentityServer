using Identity.Library.Constants;
using Identity.Library.Data;
using Identity.Library.Identity;
using Identity.Library.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityConstants = Identity.Library.Constants.IdentityConstants;

[assembly: HostingStartup(typeof(AccountManagement.Areas.Identity.IdentityHostingStartup))]
namespace AccountManagement.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services
                    .AddDbContext<ApplicationDbContext>(o =>
                    {
                        o.UseSqlServer(context.Configuration.GetConnectionString("ApplicationDbContextConnection"));
                    });

                services
                    .AddIdentity<ApplicationUser, IdentityRole>(o =>
                     {
                         o.SignIn.RequireConfirmedAccount = true;

                         o.User.RequireUniqueEmail = true;

                         o.ClaimsIdentity.RoleClaimType = IdentityConstants.RoleClaimType;

                         o.Password.RequireDigit = false;
                         o.Password.RequireLowercase = false;
                         o.Password.RequireUppercase = false;
                         o.Password.RequireNonAlphanumeric = false;
                         o.Password.RequiredUniqueChars = 1;
                         o.Password.RequiredLength = 4;
                     })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddClaimsPrincipalFactory<ApplicationUserClaimsFactory>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders()
                    .AddSignInManager<ApplicationSignInManager>();
            });
        }
    }
}