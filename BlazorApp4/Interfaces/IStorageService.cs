namespace BlazorApp4.Interfaces
{
    /// <summary>
    /// Defines methods for saving, loading, and exporting data using the browsers localstorage
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Saves an object to local storage
        /// </summary>
        /// <typeparam name="T">The type of the object being stored</typeparam>
        /// <param name="key">The unique key to identify the stored item</param>
        /// <param name="value">The object value to store</param>
        Task SetItemAsync<T>(string key, T value);

        /// <summary>
        /// Retrieves and deserializes an object from localStorage
        /// </summary>
        /// <typeparam name="T">The expected type of the stored object</typeparam>
        /// <param name="key">The unique key identifying the stored item</param>
        /// <returns>The deserialized object of type</returns>
        Task<T> GetItemAsync<T>(string key);

        /// <summary>
        /// Retrieves a stored item as a raw JSON or string value
        /// </summary>
        /// <param name="key">The unique key identifying the stored item</param>
        /// <returns>The stored value as a string</returns>
        Task<string> GetItemAsStringAsync(string key);

        /// <summary>
        /// Stores a raw string or JSON value in storage using the specified key
        /// </summary>
        /// <param name="key">The unique key to identify the stored item</param>
        /// <param name="value">The string content to store</param>
        Task SetItemAsStringAsync(string key, string value);

        /// <summary>
        /// Downloads the file with the specified name and content
        /// </summary>
        /// <param name="fileName">The name of the file to be downloaded</param>
        /// <param name="content">The content to include in the downloaded file</param>
        Task DownloadFileAsync(string fileName, string content);
        
        /// <summary>
        /// Reads the contents of a file and returns it as a string
        /// </summary>
        /// <returns>The file content as a string</returns>
        Task<string> ReadFileAsync();
    }
}