using Duende.IdentityServer.Models;
using static Duende.IdentityServer.IdentityServerConstants;

namespace RecommendCoffee.Identity.Api
{
    public class IdentityConfig
    {
        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                ClientId = "763bec1c-d89a-4321-adfd-e2db694d66a6",
                ClientSecrets = new[]
                {
                    new Secret("f638bd6a-abaa-4bd0-a57d-fcaf66e5cf85".Sha256())
                },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris= { "https://oauth.pstmn.io/v1/callback"},
                Description= "Recommend coffee portal",
                AllowAccessTokensViaBrowser = true,
                RequirePkce = false,
                AllowedScopes =
                {
                    StandardScopes.OpenId,
                    StandardScopes.Profile
                }
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("catalog:read"),
            new ApiScope("catalog:write")
        };
    }
}
