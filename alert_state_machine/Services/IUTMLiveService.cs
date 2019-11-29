using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace alert_state_machine.Services
{
    public interface IUTMLiveService
    {
        Task Connect(string token, Action<object, string, object> action);

    }
}
