using IdentityServer4.Models;
using System.Collections.Generic;

namespace Neider.IdentityServer
{
    /// <summary>
    /// https://identityserver4.readthedocs.io/en/latest/quickstarts/1_client_credentials.html
    /// </summary>
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("NeiderApi", "Neider Chat API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "NeiderApi" }
                }
            };
    }
}
