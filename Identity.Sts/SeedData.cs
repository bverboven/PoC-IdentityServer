// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Identity.Library.Data;
using Identity.Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;

namespace Identity.Sts
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services
                .AddIdentity<ApplicationUser, IdentityRole>(o =>
                {
                    o.SignIn.RequireConfirmedAccount = true;

                    o.User.RequireUniqueEmail = true;

                    o.ClaimsIdentity.RoleClaimType = "role";

                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredUniqueChars = 1;
                    o.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            var adminEmail = "bram.verboven@gmail.com";
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminUser = userMgr.FindByNameAsync(adminEmail).Result;
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(adminUser, "admin").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var adminRole = new IdentityRole("Administrator");
                roleMgr.CreateAsync(adminRole).Wait();
                foreach (var claim in new[] {
                    new Claim("permission", "can_read"),
                    new Claim("permission", "can_create"),
                    new Claim("permission", "can_update"),
                    new Claim("permission", "can_delete")
                })
                {
                    roleMgr.AddClaimAsync(adminRole, claim).Wait();
                }
                userMgr.AddToRoleAsync(adminUser, adminRole.Name).Wait();

                Log.Debug("admin created");
            }
            else
            {
                Log.Debug("admin already exists");
            }

            return;

            var bramUser = userMgr.FindByNameAsync("bram").Result;
            if (bramUser == null)
            {
                bramUser = new ApplicationUser
                {
                    UserName = "bramverboven@hotmail.com",
                    Email = "bramverboven@hotmail.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bramUser, "test").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bramUser, new[] {
                    //new Claim("role", "user"),
                    new Claim("permission", "can_read")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("bram created");
            }
            else
            {
                Log.Debug("bram already exists");
            }
        }
    }
}
