using System.Collections.Concurrent;
using TestTask.Contract.Cache;
using TestTask.Contract.Providers;
using TestTask.Services.Mappers;
using TestTask.Services.Providers.Register;

namespace TestTask;

public class SearchService : ISearchService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SearchService> _logger;
    private readonly ICacheProvider<Route> _cache;
    private List<IRouteProvider> _activeProviders = new List<IRouteProvider>();

    public SearchService(IServiceProvider serviceProvider,
                         ILogger<SearchService> logger,
                         ICacheProvider<Route> cache)
    {
        _serviceProvider = serviceProvider;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var providers = ProviderRegistration.GetProviders();

            foreach (var type in providers)
            {
                var provider = scope.ServiceProvider.GetRequiredService(type) as ISearchProvider;

                try
                {
                    await provider!.Ping();
                    _activeProviders.Add(provider);
                }
                catch (System.Exception ex)
                {
                    _logger.LogWarning($"Provider {provider!.GetType().Name} is unavailable", ex);
                }
            }

            return _activeProviders.Count() > 0;
        }
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        if (request.Filters?.OnlyCached is true)
        {
            var cachedRoute = _cache.GetAll(request.ToClause());

            return new SearchResponse().FromRoute(cachedRoute);
        }

        var state = await IsAvailableAsync(new CancellationToken());

        if (state)
        {
            var routes = new ConcurrentBag<Route>();
            var tasks = new List<Task>();

            foreach (var provider in _activeProviders)
            {
                var task = Task.Run(async () =>
            {
                var result = await provider.GetRoute(request);
                foreach (var route in result.Routes)
                {
                    routes.Add(route);
                }
            });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            await CacheRoutes(routes);

            return new SearchResponse().FromRoute(routes);
        }

        throw new Exception("No provider available");
    }

    private async Task CacheRoutes(IEnumerable<Route> routes)
    {
        var now = DateTime.UtcNow;
        foreach (var route in routes)
        {
            await _cache.Insert(route, route.TimeLimit - now);
        }
    }
}