using System;
using System.Net.Http;
using System.Threading.Tasks;
using Identity.Library.Defaults;
using Identity.Library.DependencyInjection;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace AccountConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddUserSecrets(typeof(Program).Assembly, true)
                .Build();
            var services = new ServiceCollection();
            services.LoadCommonIdentity(config.GetSection("Urls"));

            // get discovery document
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(IdentityDefaults.Authority);

            var pg = new Program();

            // get token using client credentials
            var tokenResponse = await pg.GetAccessTokenUsinClientCredentials(disco);
            Console.WriteLine($"Token: {tokenResponse.Json}");

            var apiContent1 = await pg.CallApi(tokenResponse.AccessToken);
            Console.WriteLine($"Api response: {JArray.Parse(apiContent1)}");


            // get token using pwd & username
            var pwdResponse = await pg.GetAccessTokenUsingUsernameAndPassword(disco);
            Console.WriteLine($"Pwd token: {pwdResponse.Json}");

            var content2 = await pg.CallApi(pwdResponse.AccessToken);
            Console.WriteLine($"Api response: {JArray.Parse(content2)}");


            // The End
            Console.ReadLine();
        }

        async Task<TokenResponse> GetAccessTokenUsinClientCredentials(DiscoveryDocumentResponse disco)
        {
            var tokenClient = new HttpClient();
            var clientReq = new ClientCredentialsTokenRequest
            {
                ClientId = "consoleApp",
                ClientSecret = "Console_App",
                Address = disco.TokenEndpoint,
                Scope = "accountApi"
            };
            return await tokenClient.RequestClientCredentialsTokenAsync(clientReq);
        }
        async Task<TokenResponse> GetAccessTokenUsingUsernameAndPassword(DiscoveryDocumentResponse disco)
        {
            var tokenClient = new HttpClient();
            var pwdReq = new PasswordTokenRequest
            {
                ClientId = "consoleApp",
                ClientSecret = "Console_App",
                Address = disco.TokenEndpoint,
                UserName = "bram@regira.com",
                Password = "test",
                Scope = "accountApi"
            };
            return await tokenClient.RequestPasswordTokenAsync(pwdReq);
        }
        async Task<string> CallApi(string accessToken)
        {
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(accessToken);

            var apiResponse = await apiClient.GetAsync($"{IdentityDefaults.AccountApi}/api/claims");
            return await apiResponse.Content.ReadAsStringAsync();
        }
    }
}
