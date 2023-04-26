namespace TestTask.Contract.Cache;

public interface ICacheProvider<T> where T: notnull
{
    IEnumerable<T> GetAll(Func<T, bool> filter = null);
    Task<T?> GetByKey(string key);
    Task Insert(T obj, TimeSpan time);
}