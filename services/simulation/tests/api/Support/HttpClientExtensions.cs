using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.SystemTextJson;

namespace TastyBeans.Simulation.Api.Tests.Support;

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> PostCloudEventAsync<T>(this HttpClient client, string url, string eventType, T? data)
    {
        var cloudEventInstance = new CloudEvent(CloudEventsSpecVersion.Default)
        {
            Id = Guid.NewGuid().ToString(),
            Source = new Uri(url, UriKind.Relative),
            Data = data,
            Type = eventType,
            Time = DateTimeOffset.UtcNow,
            DataContentType = "application/json"
        };

        var jsonEventFormatter = new JsonEventFormatter<T>();

        var requestContent = new ReadOnlyMemoryContent(
            jsonEventFormatter.EncodeStructuredModeMessage(
                cloudEventInstance, out var eventContentType));

        requestContent.Headers.ContentType = new MediaTypeHeaderValue(eventContentType.MediaType);

        var response = await client.PostAsync(url, requestContent);

        return response;
    }
}