namespace TestTask.Services.Mappers;

public static class ProviderTwoMapper
{
    public static ProviderTwoSearchRequest MapProviderTwoRequest(this SearchRequest source)
    {
        return new ProviderTwoSearchRequest()
        {
            Departure = source.Origin,
            Arrival = source.Destination,
            DepartureDate = source.OriginDateTime,
            MinTimeLimit = source.Filters?.MinTimeLimit
        };
    }

    public static Route MapRoute(this ProviderTwoRoute source)
    {
        return new Route()
        {
            Origin = source.Departure.Point,
            Destination = source.Arrival.Point,
            OriginDateTime = source.Departure.Date,
            DestinationDateTime = source.Arrival.Date,
            Price = source.Price,
            TimeLimit = source.TimeLimit
        };
    }

    public static SearchResponse MapResponse(this ProviderTwoRoute[] source)
    {
        var routes = source.Select(x => x.MapRoute());

        return new SearchResponse()
        {
            Routes = routes.ToArray()
        };
    }
}