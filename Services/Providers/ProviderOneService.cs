using TestTask.Contract;
using TestTask.Contract.Providers;
using TestTask.Services.Mappers;
using TestTask.Services.Providers.Base;

namespace TestTask.Services.Providers;

public class ProviderOneService : BaseWebProvider, ISearchProvider
{
    public async Task<SearchResponse> GetRoute(SearchRequest request)
    {
        var @args = request.MapProviderOneRequest();
        var response = await _client.PostAsJsonAsync("http://provider-one/api/v1/search", args);
        var result = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>();

        return result!.Routes.MapResponse();
    }

    public override async Task Ping()
    {
        var result = await _client.GetAsync("http://provider-one/api/v1/ping");
        result.EnsureSuccessStatusCode();
    }
}