
using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorApp4.Services
{
    /// <summary>
    /// Provides access to browser-based storage and file
    /// This service enables reading and writing data to localstorage
    /// downloading files, and reading uploaded files from the user's browser   
    /// </summary>
    public class StorageService : IStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        private JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// Creates a new instance of the StorageService
        /// </summary>
        /// <param name="jsRuntime">JS runtime used for interacting with localStorage</param>
        public StorageService(IJSRuntime jsRuntime) => _jsRuntime = jsRuntime;

        /// <summary>
        /// Gets and deserializes a value from localStorage
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve</typeparam>
        /// <param name="key">The key under which the object is stored</param>
        /// <returns>The deserialized object or default if not found</returns>
        public async Task<T> GetItemAsync<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions)!;
        }

        /// <summary>
        /// Stores an object in localStorage after serializing it to JSON
        /// </summary>
        /// <typeparam name="T">The type of object to store</typeparam>
        /// <param name="key">The key to store the object under</param>
        /// <param name="value">The object to serialize and store</param>
        public async Task SetItemAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value, _jsonSerializerOptions);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);

        }

        /// <summary>
        /// Stores a plain string value in localStorage
        /// </summary>
        /// <param name="key">Storage key name</param>
        /// <param name="value">String value to store</param>
        public async Task SetItemAsStringAsync(string key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        /// <summary>
        /// Retrieves a plain string value from localStorage
        /// </summary>
        /// <param name="key">Storage key name</param>
        /// <returns>The stored string value or an empty string if not found</returns>
        public async Task<string> GetItemAsStringAsync(string key)
        {
            var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            return value ?? string.Empty;
        }

        /// <summary>
        /// Initiates a file download in the user's browser
        /// </summary>
        /// <param name="fileName">The name of the file to be downloaded</param>
        /// <param name="content">The file content as string</param>
        /// <returns></returns>
        public async Task DownloadFileAsync(string fileName, string content)
        {
            await _jsRuntime.InvokeVoidAsync("downloadFileFromContent", fileName, content);
        }

        /// <summary>
        /// Reads the content of a file selected by the user
        /// </summary>
        /// <returns>The file content as a string</returns>
        public async Task<string> ReadFileAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("readFileContent");
        }


    }
}