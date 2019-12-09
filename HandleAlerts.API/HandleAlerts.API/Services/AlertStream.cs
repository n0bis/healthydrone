using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using HandleAlerts.API.Domain.Models;
using HandleAlerts.API.Domain.Services;
using System.Linq;

namespace HandleAlerts.API.Services
{
    public class AlertStream : IAlertStream, IDisposable
    {
        private Subject<Alert> _alertSubject;
        private IDictionary<string, IDisposable> _subscribers;

        public AlertStream()
        {
            _alertSubject = new Subject<Alert>();
            _subscribers = new Dictionary<string, IDisposable>();
        }

        public void Publish(Alert alert)
        {
            _alertSubject.OnNext(alert);
        }

        public void Subscribe(string subscriberName, Action<Alert> action)
        {
            if(!_subscribers.ContainsKey(subscriberName))
            {
                _subscribers.Add(subscriberName, _alertSubject.Subscribe(action));
            }
        }

        public void Dispose()
        {
            if (_alertSubject != null)
            {
                _alertSubject.Dispose();
            }

            foreach (var subscriber in _subscribers)
            {
                subscriber.Value.Dispose();
            }
        }
    }
}
