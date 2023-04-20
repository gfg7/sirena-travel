namespace TestTask.Contract.Cache;

public interface ICacheProvider<T> where T: notnull
{
    Task Upsert(string key, T obj, TimeSpan? time=null);
    Task Get(string key, IQueryable<T> filter = null);
}