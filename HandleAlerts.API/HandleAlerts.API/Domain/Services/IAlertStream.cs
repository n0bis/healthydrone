using System;
using HandleAlerts.API.Domain.Models;

namespace HandleAlerts.API.Domain.Services
{
    public interface IAlertStream
    {
        void Publish(Alert alert);

        void Subscribe(string subscriberName, Action<Alert> action);
    }
}
