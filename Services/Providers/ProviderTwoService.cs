using sirena_travel.Services;
using TestTask.Contract;
using TestTask.Contract.Providers;
using TestTask.Services.Mappers;
using TestTask.Services.Providers.Base;

namespace TestTask.Services.Providers;

public class ProviderTwoService : BaseWebProvider, ISearchProvider
{
    public ProviderTwoService(ILogger<ProviderTwoService> logger) : base(logger)
    {
    }

    public async Task<SearchResponse> GetRoute(SearchRequest request)
    {
        var @args = request.MapProviderTwoRequest();
        _logger.LogInformation($"Get route from {_client.BaseAddress}");
        var response = await _client.PostAsJsonAsync(EnvironmentUtil.Get("ProviderTwoService"), args);
        var result = await response.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>();


        return result!.Routes.MapResponse();
    }

    public override async Task Ping()
    {
        var result = await _client.GetAsync(EnvironmentUtil.Get("ProviderTwoPing"));
        _logger.LogInformation($"Ping {_client.BaseAddress} {result.RequestMessage.RequestUri} {result.StatusCode}");
        result.EnsureSuccessStatusCode();
    }

    protected override HttpClient SetupClient()
    {
        return new HttpClient()
        {
            BaseAddress = new Uri(EnvironmentUtil.Get("ProviderTwo"))
        };
    }
}