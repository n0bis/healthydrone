using System;
using System.Threading.Tasks;
using alert_state_machine.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HandleAlerts.API.Persistence
{
    public class RedisService : IRedisService
    {
        private readonly string host;
        private readonly int port;
        private ConnectionMultiplexer redis;

        public RedisService(IOptions<RedisOpts> options)
        {
            this.host = options.Value.Host;
            this.port = options.Value.Port;
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
                    return default;
                }
            } catch(Exception)
            {
                throw;
            }
        }
    }
}