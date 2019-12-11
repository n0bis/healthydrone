using System;
using HandleAlerts.API.Domain.Services;
using HandleAlerts.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HandleAlerts.API.Services
{
    public class AlertRelay
    {
        public AlertRelay(IHubContext<AlertHub> hubContext, IAlertStream alertStream)
        {
            alertStream.Subscribe("alert_relay", async (alert) => await hubContext.Clients.All.SendAsync("alerts", alert.Message));
        }
    }
}
