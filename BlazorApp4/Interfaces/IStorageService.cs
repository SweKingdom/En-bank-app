namespace BlazorApp4.Interfaces
{
    public interface IStorageService
    {
        // Save
        Task SetItemAsync<T>(string key, T value);
        // Load
        Task<T> GetItemAsync<T>(string key);

        Task<string> GetItemAsStringAsync(string key);
        Task SetItemAsStringAsync(string key, string value);

        Task DownloadFileAsync(string fileName, string content);

        Task<string> ReadFileAsync();
    }
}