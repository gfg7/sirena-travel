namespace TestTask.Services.Mappers;

public static class SearchReponseBuilder
{
    public static SearchResponse FromRoute(this SearchResponse source, IEnumerable<Route> routes)
    {
        if (routes.Count() > 0)
        {
            source.Routes = routes.ToArray();
            source.MinPrice = routes.Min(x => x.Price);
            source.MaxPrice = routes.Max(x => x.Price);
            source.MinMinutesRoute = routes.Min(x => (x.DestinationDateTime - x.OriginDateTime).Minutes);
            source.MaxMinutesRoute = routes.Max(x => (x.DestinationDateTime - x.OriginDateTime).Minutes);
        }

        return source;
    }
}