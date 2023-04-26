using System.Linq.Expressions;
namespace TestTask.Services.Mappers;

public static class SearchRequestClauseBuilder
{
    public static Func<Route, bool> ToClause(this SearchRequest request)
    {
        return x =>
        x.Destination == request.Destination &&
        x.Origin == request.Origin &&
        x.OriginDateTime <= request.OriginDateTime &&
        (request.Filters is not null &&
            (request.Filters.DestinationDateTime.HasValue && x.DestinationDateTime == request.Filters.DestinationDateTime ||
            request.Filters.MaxPrice.HasValue && x.Price <= request.Filters.MaxPrice ||
            request.Filters.MinTimeLimit.HasValue && x.TimeLimit >= request.Filters.MinTimeLimit
            )
        );
    }
}