using TestTask.Contract;
using TestTask.Contract.Providers;
using TestTask.Services.Mappers;
using TestTask.Services.Providers.Base;

namespace TestTask.Services.Providers;

public class ProviderTwoService : BaseWebProvider, ISearchProvider
{
    public async Task<SearchResponse> GetRoute(SearchRequest request)
    {
        var @args = request.MapProviderTwoRequest();
        var response = await _client.PostAsJsonAsync("http://provider-two/api/v1/search", args);
        var result = await response.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>();

        return result!.Routes.MapResponse();
    }

    public override async Task Ping()
    {
        var result = await _client.GetAsync("http://provider-two/api/v1/ping");
        result.EnsureSuccessStatusCode();
    }
}