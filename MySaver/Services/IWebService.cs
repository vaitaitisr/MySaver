namespace MySaver.Services;

public interface IWebService
{
    Task<List<T>> GetObjectListAsync<T>();
}