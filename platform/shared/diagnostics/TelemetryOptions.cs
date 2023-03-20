namespace TastyBeans.Shared.Diagnostics;

public class TelemetryOptions
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string TracingEndpoint { get; set; }
    public string LoggingEndpoint { get; set; }
}