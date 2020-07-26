using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Neider.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public  void OnGet()
        {
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync("https://localhost:44311").GetAwaiter().GetResult();
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }
            // request token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "NeiderApi"
            }).GetAwaiter().GetResult();

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = apiClient.GetAsync("https://localhost:44310/identity").GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                Console.WriteLine(JArray.Parse(content));
            }
        }
    }
}
