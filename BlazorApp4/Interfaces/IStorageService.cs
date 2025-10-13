namespace BlazorApp4.Interfaces
{
    public interface IStorageService
    {
        //Spara
        Task SetItemAsync<T>(string key, T value);
        //Hämta
        Task<T> GetItemsAsync<T>(string key);

    }
}
