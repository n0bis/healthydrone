using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace alert_state_machine.Persistence
{
    public class RedisService
    {
        private readonly string host;
        private readonly int port;
        private ConnectionMultiplexer redis;

        public RedisService(IConfiguration config)
        {
            this.host = config["Redis:Host"] ?? "localhost";
            this.port = Convert.ToInt32(config["Redis:Port"] ?? "6379");
        }

        public void Connect()
        {
            try
            {
                var configString = $"{this.host}:{this.port},connectRetry=5";
                this.redis = ConnectionMultiplexer.Connect(configString);
            }
            catch (RedisConnectionException e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task Set(string key, object value)
        {
            var db = this.redis.GetDatabase();
            await db.StringSetAsync(key, JsonConvert.SerializeObject(value), new TimeSpan(24, 0, 0));
        }

        public async Task<T> Get<T>(string key)
        {
            var db = this.redis.GetDatabase();
            try
            {
                var value = await db.StringGetAsync(key);
                if (!value.IsNull)
                {
                    return JsonConvert.DeserializeObject<T>(value);
                } else {
                    return default(T);
                }
            } catch(Exception)
            {
                throw;
            }
        }

        public async Task<bool> Set(string key, string value)
        {
            var db = this.redis.GetDatabase();
            return await db.StringSetAsync(key, value, new TimeSpan(24,0,0));
        }

        public async Task<string> Get(string key)
        {
            var db = this.redis.GetDatabase();
            return await db.StringGetAsync(key);
        }
    }
}