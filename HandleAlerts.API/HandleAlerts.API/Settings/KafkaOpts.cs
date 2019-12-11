using System;
namespace HandleAlerts.API.Settings
{
    public class KafkaOpts
    {
        public string Host { get; set; }
        public string Topic { get; set; }
        public string GroupId { get; set; }
    }
}
