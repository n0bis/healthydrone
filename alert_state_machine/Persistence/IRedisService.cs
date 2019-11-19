using System;
using System.Threading.Tasks;

namespace alert_state_machine.Persistence
{
    public interface IRedisService
    {
        void Connect();
        Task Set(string key, object value);
        Task<T> Get<T>(string key);
    }
}
