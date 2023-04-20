using TestTask.Contract;

namespace TestTask.Services.Mappers;

public static class ProviderOneMapper
{
    public static ProviderOneSearchRequest MapProviderOneRequest(this SearchRequest source)
    {
        return new ProviderOneSearchRequest()
        {
            From = source.Origin,
            To = source.Destination,
            DateFrom = source.OriginDateTime,
            DateTo = source.Filters?.DestinationDateTime,
            MaxPrice = source.Filters?.MaxPrice
        };
    }

    public static Route MapRoute(this ProviderOneRoute source)
    {
        return new Route
        {
            Destination = source.To,
            Origin = source.From,
            TimeLimit = source.TimeLimit,
            Price = source.Price,
            DestinationDateTime = source.DateTo,
            OriginDateTime = source.DateFrom
        };
    }

    public static SearchResponse MapResponse(this ProviderOneRoute[] source)
    {
        var routes = source.Select(x => x.MapRoute());

        return new SearchResponse()
        {
            Routes = routes.ToArray()
        };
    }
}