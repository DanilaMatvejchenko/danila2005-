using ElectronicJournal.Web.Models;

namespace ElectronicJournal.Web.Services;

public interface IApiService
{
    Task<T?> GetAsync<T>(string url);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data);
    Task<bool> PostAsync<TRequest>(string url, TRequest data);
    Task<bool> PutAsync<TRequest>(string url, TRequest data);
    Task<bool> DeleteAsync(string url);
}
