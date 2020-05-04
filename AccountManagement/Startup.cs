using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Library.Constants;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using IdentityConstants = Identity.Library.Constants.IdentityConstants;

namespace AccountManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = IdentityConstants.AuthenticationScheme;// required when using OpenIdConnect to prevent infinite redirect loop
                    o.DefaultChallengeScheme = IdentityConstants.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {

                })
                .AddOpenIdConnect(IdentityConstants.AuthenticationScheme, o =>
                {
                    o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;// required when using OpenIdConnect to prevent infinite redirect loop
                    o.ClientId = "accountManager";
                    o.ClientSecret = "Account_Management";
                    o.Authority = IdentityConstants.Authority;
                    o.CallbackPath = "/signin-oidc";

                    o.UsePkce = true;
                    o.SaveTokens = true;
                    o.ResponseType = OpenIdConnectParameterNames.Code;
                    o.ResponseMode = "form_post";
                    o.GetClaimsFromUserInfoEndpoint = true;

                    //o.Scope.Add("openid");
                    //o.Scope.Add("profile");
                    o.Scope.Add("roles");
                    o.ClaimActions.MapUniqueJsonKey(IdentityConstants.RoleClaimType, "role");
                    o.Scope.Add("permissions");
                    o.ClaimActions.MapUniqueJsonKey("permission", "permission", "json");

                    o.ClaimActions.DeleteClaim("s_hash");

                    o.ClaimActions.MapUniqueJsonKey(JwtClaimTypes.AuthenticationMethod, JwtClaimTypes.AuthenticationMethod);

                    // required to enable Roles
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = IdentityConstants.RoleClaimType
                    };
                });

            services
                .AddAuthorization(o =>
                 {
                     o.AddPolicy("Custom", c =>
                     {
                         c.RequireAuthenticatedUser();
                         c.AddRequirements(new HasAccessRequirement());
                     });
                     o.AddPolicy("HasPermission", c =>
                     {
                         c.RequireAuthenticatedUser();
                         c.RequireClaim("permission");
                     });
                     o.AddPolicy("CanRead", c =>
                     {
                         c.RequireAuthenticatedUser();
                         c.RequireClaim("permission", "can_read");
                     });
                     o.AddPolicy("IsAdmin", c =>
                     {
                         c.RequireAuthenticatedUser();
                         c.RequireRole("Administrator");
                     });
                 });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
                    .RequireAuthorization();
                endpoints.MapRazorPages();
            });
        }
    }

    public class HasAccessRequirement : IAuthorizationRequirement
    {
    }

    public class HasAccessRequirementHandler : AuthorizationHandler<HasAccessRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasAccessRequirement requirement)
        {
            var claims = context.User.Claims;

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
