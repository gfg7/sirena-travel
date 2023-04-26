namespace TestTask.Middleware;

public class LoggingHandler
{
    private RequestDelegate _request;
    private ILogger<LoggingHandler> _logger;

    public LoggingHandler(RequestDelegate request, ILogger<LoggingHandler> logger)
    {
        _logger = logger;
        _request = request;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await LogRequest(context.Request);
            await _request.Invoke(context);

        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            context.Response.StatusCode = 500;
        }
        finally
        {
            await LogResponse(context);
        }
    }

    private async Task LogResponse(HttpContext context)
    {
        string log = $"Reponse: {context.Request.Method} {context.Request.Path} {context.Response.StatusCode}\n";
        _logger.LogInformation(log);
    }

    private async Task LogRequest(HttpRequest request)
    {
        string log = $"Request: {request.Method} {request.Path}\n";
        log += $"Headers: \n";
        request.Headers.ToList().ForEach(x => log += $"{x.Key}={x.Value}\n");
        log += $"Params: {request.QueryString.Value}\n";

        _logger.LogInformation(log);
    }
}