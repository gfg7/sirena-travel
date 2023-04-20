using TestTask.Contract.Providers;

namespace TestTask.Services.Providers.Base;

public abstract class BaseWebProvider : IPing
{
    protected readonly HttpClient _client;
    public BaseWebProvider()
    {
        _client = SetupClient();
    }

    public abstract Task Ping();
    protected virtual HttpClient SetupClient() {
        return new HttpClient();
    }
}