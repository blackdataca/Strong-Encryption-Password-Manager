using MyIdMobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyIdMobile.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

        Task<bool> LoadFromDiskAsync(string pPrivateKeyFile = null);
        Task SaveToDiskAsync(bool webSync = true);
        Task<bool> WebSync();

        List<Item> AllItems { get; set; }
    }
}
