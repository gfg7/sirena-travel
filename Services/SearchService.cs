using Microsoft.Extensions.Caching.Memory;
using TestTask.Contract.Providers;
using TestTask.Services.Providers.Register;

namespace TestTask;

public class SearchService : ISearchService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SearchService> _logger;
    private IRouteProvider _provider;

    public SearchService(IMemoryCache memoryCache, IServiceProvider serviceProvider, ILogger<SearchService> logger)
    {
        _serviceProvider = serviceProvider;
        _memoryCache = memoryCache;
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
                    _provider = provider;
                    return true;
                }
                catch (System.Exception ex)
                {
                    _logger.LogWarning($"Provider {provider!.GetType().Name} is unavailable", ex);
                }
            }

            _logger.LogError($"No provider available");
            return false;
        }
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        if (request.Filters?.OnlyCached.HasValue is true)
        {
            // add cache filter logic
            return null;
        }

        var state = await IsAvailableAsync(new CancellationToken());

        if (state)
        {
            var result = await _provider!.GetRoute(request);
            return result;
        }

        throw new Exception("No provider available");
    }
}