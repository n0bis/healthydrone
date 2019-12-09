using System;
using System.Threading.Tasks;

namespace HandleAlerts.API.Domain.Services
{
    public interface IAlertConsumer
    {
        void Listen();
    }
}
