using MatchMaking.Common.Types;
using Microsoft.Extensions.Configuration;

using StackExchange.Redis;

namespace MatchMaking.Common.Redis
{
    public class RedisCacheClient(IConfiguration configuration) : ICacheClient
    {
        private IConnectionMultiplexer _conn;
        private IDatabase _db;

        private IConnectionMultiplexer GetConnection()
        {
            if (_conn == null || !_conn.IsConnected)
            {
                var connectionString = configuration.GetSection("Redis")["ConnectionString"];
                _conn = ConnectionMultiplexer.Connect(connectionString);
            }

            return _conn;
        }

        private IDatabase GetDatabase()
        {
            if (_db == null)
            {
                var connection = GetConnection();
                _db = connection.GetDatabase();
            }

            return _db;
        }

        public Task Init()
        {
            _ = GetDatabase();
            return Task.CompletedTask;
        }

        public async Task<bool> AddToSet(string setKey, string val)
        {
            var db = GetDatabase();
            return await db.SetAddAsync(setKey, val);
        }

        public async Task<long> AddToSet(string setKey, string[] members)
        {
            var db = GetDatabase();
            RedisValue[] redisValues = Array.ConvertAll(members, x => (RedisValue)x);
            return await db.SetAddAsync(setKey, redisValues);
        }

        public async Task<string[]> GetSetMembersByKey(string setKey)
        {
            var db = GetDatabase();
            var vals = await db.SetMembersAsync(setKey);
            return vals.Select(x => x.ToString()).ToArray();
        }

        public async Task<string> GetByKey(string key)
        {
            var db = GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<bool> SetByKey(string key, string val)
        {
            var db = GetDatabase();
            return await db.StringSetAsync(key, val);
        }

        public async Task<bool> SetByKey(string key, string val, TimeSpan? ttl)
        {
            var db = GetDatabase();
            return await db.StringSetAsync(key, val, ttl);
        }

        public async Task DeleteKey(string key)
        {
            var db = GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
