using Duende.IdentityServer.Models;

namespace RecommendCoffee.Identity.Api;

public static class Configuration
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        yield return new IdentityResources.OpenId();
        yield return new IdentityResources.Profile();
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        yield return new ApiResource("catalog", "Product Catalog");
        yield return new ApiResource("customermanagement", "Customer Management");
        yield return new ApiResource("payments", "Payments");
        yield return new ApiResource("ratings", "Product Ratings");
        yield return new ApiResource("registration", "Registration");
        yield return new ApiResource("subscriptions", "Subscriptions");
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
                "customermanagement",
                "payments",
                "subscriptions"
            }
        };
        
        yield return new Client
        {
            ClientId = "8766ded8-f31e-48c2-bc5c-03a500f2e491",
            ClientSecrets = new[] {
                new Secret("e398d7b1-ab8a-4a31-89ef-3a3febdc8afd".Sha256())
            },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris =
            {
                "https://localhost:6001/signin-callback-oidc",
                "https://recommend.coffee/signin-callback-oidc"
            },
            PostLogoutRedirectUris =
            {
                "https://localhost:6001/signout-callback-oidc",
                "https://recommend.coffee/signout-callback-oidc"
            },
            AllowAccessTokensViaBrowser = true,
            RequirePkce = false,
            AllowedScopes =
            {
                "catalog",
                "customermanagement",
                "payments",
                "ratings",
                "registration",
                "subscriptions",
                "openid", 
                "profile"
            }
        };
    }
}