using Domain.CustomExceptions;
using StackExchange.Redis;
using System.Text.Json;

namespace Application.Services{
    public interface IRedisService {
        public Task SetCodeAsync(string email, string code, int expiryMinutes);
        public Task SetObjectAsync<T>(string key, T value, int expiryMinutes);
        public Task SetStringAsync(string key, string value, int expiryMinutes = 15);
        public Task<string?> GetStringAsync(string key);
        public Task<T?> GetObjectAsync<T>(string key);
        public Task<string?> GetCodeAsync(string email);
        public Task<bool> DeleteAsync(string key); 
        public Task<bool> ValidateAndDeleteCodeAsync(string email, string code);
    }
    public class RedisService : IRedisService{
        private readonly IDatabase _db;

        public RedisService(ConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetCodeAsync(string email, string code, int expiryMinutes){
            await _db.StringSetAsync(email, code, TimeSpan.FromMinutes(expiryMinutes));
        }
        
        public async Task SetObjectAsync<T>(string key, T value, int expiryMinutes){
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, TimeSpan.FromMinutes(expiryMinutes));
        }

        public async Task<string?> GetCodeAsync(string email){
            return await _db.StringGetAsync(email);
        }

        public async Task<bool> ValidateAndDeleteCodeAsync(string email, string code){
            var c = await GetCodeAsync(email) 
                ?? throw new NotFoundException("Could not find the given code for the specified key");
            
            bool result = c == code;
            if (result) await _db.KeyDeleteAsync(email);
            return result;
        }
        public async Task<T?> GetObjectAsync<T>(string key){
            var json = await _db.StringGetAsync(key);   
            return json.HasValue ? JsonSerializer.Deserialize<T>(json!) : default;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
            return true;
        }

        public Task SetStringAsync(string key, string value, int expiryMinutes = 15)
        {
            return _db.StringSetAsync(key, value, TimeSpan.FromMinutes(expiryMinutes));
        }

        public async Task<string?> GetStringAsync(string key)
        {
            var result = await _db.StringGetAsync(key);
            return result.HasValue ? result.ToString() : null;
        }
    }
}