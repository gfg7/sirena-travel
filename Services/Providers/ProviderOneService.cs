using sirena_travel.Services;
using TestTask.Contract;
using TestTask.Contract.Providers;
using TestTask.Services.Mappers;
using TestTask.Services.Providers.Base;

namespace TestTask.Services.Providers;

public class ProviderOneService : BaseWebProvider, ISearchProvider
{
    public ProviderOneService(ILogger<ProviderOneService> logger) : base(logger)
    {
    }

    public async Task<SearchResponse> GetRoute(SearchRequest request)
    {
        var @args = request.MapProviderOneRequest();
        _logger.LogInformation($"Get route from {_client.BaseAddress}");
        var response = await _client.PostAsJsonAsync(EnvironmentUtil.Get("ProviderOneService"), args);

        var result = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>();


        return result!.Routes.MapResponse();
    }

    public override async Task Ping()
    {
        var result = await _client.GetAsync(EnvironmentUtil.Get("ProviderOnePing"));

        _logger.LogInformation($"Ping {_client.BaseAddress} {result.RequestMessage.RequestUri} {result.StatusCode}");

        result.EnsureSuccessStatusCode();
    }

    protected override HttpClient SetupClient()
    {
        return new HttpClient()
        {
            BaseAddress = new Uri(EnvironmentUtil.Get("ProviderOne"))
        };
    }
}