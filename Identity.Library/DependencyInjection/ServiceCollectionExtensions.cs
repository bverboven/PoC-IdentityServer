using Identity.Library.Defaults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Library.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadCommonIdentity(this IServiceCollection services, IConfigurationSection config)
        {
            var authority = config["Authority"];
            var accountManagement = config["AccountManagement"];
            var accountApi = config["accountApi"];

            if (!string.IsNullOrWhiteSpace(authority))
            {
                IdentityDefaults.Authority = authority;
            }
            if (!string.IsNullOrWhiteSpace(accountManagement))
            {
                IdentityDefaults.AccountManagement = accountManagement;
            }
            if (!string.IsNullOrWhiteSpace(accountApi))
            {
                IdentityDefaults.AccountApi = accountApi;
            }

            return services;
        }
    }
}
