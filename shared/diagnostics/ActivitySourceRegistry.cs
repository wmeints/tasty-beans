using System.Diagnostics;

namespace RecommendCoffee.Shared.Diagnostics;

public class ActivitySourceRegistry
{
    private readonly Dictionary<string, ActivitySource> _sources = new();

    public ActivitySource Get(string name)
    {
        if (!_sources.TryGetValue(name, out var source))
        {
            source = new ActivitySource(name);
            _sources.Add(name, source);
        }

        return source;
    }
}