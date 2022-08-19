using Polly;
using Polly.Extensions.Http;

namespace TastyBeans.Registration.Api.Infrastructure.Clients;

public static class Policies
{
    public static AsyncPolicy<HttpResponseMessage> GetExponentialBackOffPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(x => (int)x.StatusCode >= 500)
            .WaitAndRetryAsync(6, attempt => TimeSpan.FromSeconds(Math.Pow(3, attempt)));
}