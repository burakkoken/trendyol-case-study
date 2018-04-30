using BestProductsApp.Services.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BestProductsApp.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly CacheServiceOptions _options;
        public CacheService(IOptions<CacheServiceOptions> options)
        {
            this._options = options?.Value;
        }

        public CacheService(string connectionStrings)
        {
            this._options = new CacheServiceOptions()
            {
                ConnectionString = connectionStrings
            };
        }

        private IDatabase _database;
        public IDatabase Database => _database != null ? _database : _database = Connect();

        private IDatabase Connect()
        {
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string cacheConnection = this._options.ConnectionString;
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
            return lazyConnection.Value.GetDatabase();
        }


        public bool Set<T>(string key, T value)
        {
            return this.Database.StringSet(key, JsonConvert.SerializeObject(value));
        }

        public async Task<bool> SetAsync<T>(string key, T value)
        {
            return await this.Database.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        public T Get<T>(string key)
        {
            try
            {
                string data = Database.StringGet(key);
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch
            {
                return default(T);
            }
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                string data = await Database.StringGetAsync(key);
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch
            {
                return default(T);
            }
        }

        public void Remove(string key)
        {
            Database.KeyDelete(key);
        }

        public async Task RemoveAsync(string key)
        {
            await Database.KeyDeleteAsync(key);
        }

        public void Clear()
        {
            foreach (var endpoint in Database.Multiplexer.GetEndPoints(true))
            {
                try
                {
                    Database.Multiplexer.GetServer(endpoint).FlushDatabase(Database.Database);
                }
                catch { }
            }
        }

        public async Task ClearAsync()
        {
            foreach (var endpoint in Database.Multiplexer.GetEndPoints(true))
            {
                try
                {
                   await Database.Multiplexer.GetServer(endpoint).FlushDatabaseAsync(Database.Database);
                }
                catch { }
            }
        }
    }
}
