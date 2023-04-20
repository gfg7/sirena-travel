namespace TestTask.Contract.Providers;

public interface IRouteProvider
{
    Task<SearchResponse> GetRoute(SearchRequest request);
}