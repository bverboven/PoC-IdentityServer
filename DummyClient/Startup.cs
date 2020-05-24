using Identity.Library.Defaults;
using Identity.Library.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace DummyClient
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

            services
                .LoadCommonIdentity(Configuration.GetSection("Urls"))
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.DefaultAuthenticateScheme = IdentityDefaults.AuthenticationScheme;// required when using OpenIdConnect to prevent infinite redirect loop
                    o.DefaultChallengeScheme = IdentityDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o => { })
                .AddOpenIdConnect(IdentityDefaults.AuthenticationScheme, o =>
                {
                    o.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    o.ClientId = "dummyClient";
                    o.ClientSecret = "Dummy_Client";
                    o.Authority = IdentityDefaults.Authority;
                    o.CallbackPath = "/signin-oidc";

                    o.UsePkce = true;
                    o.SaveTokens = true;
                    o.ResponseType = OpenIdConnectParameterNames.Code;
                    o.ResponseMode = "form_post";
                    o.GetClaimsFromUserInfoEndpoint = true;

                    o.Scope.Add("roles");
                    o.ClaimActions.MapUniqueJsonKey(IdentityDefaults.RoleClaimType, IdentityDefaults.RoleClaimType);
                    o.Scope.Add("permissions");
                    o.ClaimActions.MapUniqueJsonKey("permission", "permission", "json");

                    o.AccessDeniedPath = "/Home/AccessDenied";

                    // required to enable Roles
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = IdentityDefaults.RoleClaimType
                    };
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
                    .MapDefaultControllerRoute()
                    .RequireAuthorization();
            });
        }
    }
}
