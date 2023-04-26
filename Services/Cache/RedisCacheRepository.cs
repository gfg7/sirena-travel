using Redis.OM;
using Redis.OM.Contracts;
using sirena_travel.Services;
using TestTask.Contract.Cache;

namespace TestTask.Services.Cache;

public class RedisCacheRepository<T> : ICacheProvider<T> where T : notnull
{
    private readonly IRedisConnectionProvider _provider;
    public RedisCacheRepository()
    {
        _provider = new RedisConnectionProvider(EnvironmentUtil.Get("REDIS_CONNECTION"));

        if (CreateIndex(typeof(T)).Result)
        {
            _provider.Connection.CreateIndex(typeof(T));
        }
    }

    private async Task<bool> CreateIndex(Type type)
    {
        var index = await _provider.Connection.GetIndexInfoAsync(type);
        return index is null;
    }

    public async Task<T?> GetByKey(string key)
    {
        return await _provider.RedisCollection<T>().FindByIdAsync(key);
    }

    public IEnumerable<T> GetAll(Func<T, bool> filter = null)
    {
        if (filter is null)
        {
            return _provider.RedisCollection<T>();
        }

        return _provider.RedisCollection<T>().Where(filter);
    }

    public async Task Insert(T obj, TimeSpan time)
    {
        await _provider.Connection.SetAsync(obj,time);
    }
}