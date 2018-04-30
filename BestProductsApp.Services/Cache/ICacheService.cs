using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BestProductsApp.Services.Cache
{
    public interface ICacheService
    {
        IDatabase Database { get; }
        List<string> GetAllKeys(string search = "");
        bool Set<T>(string key, T value);
        Task<bool> SetAsync<T>(string key, T value);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        void Remove(string key);
        Task RemoveAsync(string key);
        void Clear();
        Task ClearAsync();
    }
}
