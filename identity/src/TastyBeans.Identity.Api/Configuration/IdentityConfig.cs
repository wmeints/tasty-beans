using Duende.IdentityServer.Models;

namespace TastyBeans.Identity.Api.Configuration;

public static class IdentityConfig
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        yield return new IdentityResources.OpenId();
        yield return new IdentityResources.Profile();
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        yield return new ApiResource("payments", "Payments");
        yield return new ApiResource("profiles", "User Profile");
        yield return new ApiResource("registration", "Registration");
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        yield return new ApiScope("payments", "Payments");
        yield return new ApiScope("profiles", "User Profile");
        yield return new ApiScope("registration", "Registration");
    }

    public static IEnumerable<Client> GetClients()
    {
        yield return new Client
        {
            ClientId = "930fd9d1-f79e-4318-90b0-4881e1130525",
            ClientName = "Registration API",
            ClientSecrets = new[] {
                new Secret("e398d7b1-ab8a-4a31-89ef-3a3febdc8afd".Sha256())
            },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes =
            {
                "profiles",
                "payments",
                "subscriptions"
            }
        };
        
        yield return new Client
        {
            ClientId = "8766ded8-f31e-48c2-bc5c-03a500f2e491",
            RequireClientSecret = false,
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris =
            {
                "https://localhost:3000/authentication/login-callback",
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:3000/authentication/logout-callback",
            },
            AllowedCorsOrigins =
            {
                "https://localhost:3000",
            },
            AllowAccessTokensViaBrowser = true,
            RequirePkce = false,
            AllowedScopes =
            {
                "profiles",
                "registration",
                "subscriptions",
                "openid", 
                "profile"
            }
        };
    }
}