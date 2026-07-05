namespace ConvenienceStore.Application.Contracts.External;

public interface ICacheService
{
    Task SetAsync(string key, string value, TimeSpan expiration);
    Task<string?> GetAsync(string key);
    Task RemoveAsync(string key);
}
