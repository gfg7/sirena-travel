using TestTask.Contract.Providers;

namespace TestTask.Services.Providers.Base;

public abstract class BaseWebProvider : IPing
{
    protected readonly ILogger<BaseWebProvider> _logger;
    protected readonly HttpClient _client;
    public BaseWebProvider(ILogger<BaseWebProvider> logger)
    {
        _logger = logger;
        _client = SetupClient();
    }

    public abstract Task Ping();
    protected virtual HttpClient SetupClient() {
        return new HttpClient();
    }
}