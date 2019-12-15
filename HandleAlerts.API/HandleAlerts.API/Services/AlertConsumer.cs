using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using HandleAlerts.API.Domain.Models;
using HandleAlerts.API.Domain.Services;
using HandleAlerts.API.Settings;
using Microsoft.Extensions.Options;

namespace HandleAlerts.API.Services
{
    public class AlertConsumer : IAlertConsumer
    {
        private readonly IAlertStream _alertStream;
        private readonly ConsumerConfig _kafkaConfig;
        private readonly List<string> _kafkaTopics;

        public AlertConsumer(IOptions<KafkaOpts> kafkaOptions, IAlertStream alertStream)
        {
            _kafkaConfig = new ConsumerConfig
            {
                GroupId = kafkaOptions.Value.GroupId,
                BootstrapServers = kafkaOptions.Value.Host,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _kafkaTopics = kafkaOptions.Value.Topic.Split(",").ToList();
            _alertStream = alertStream;
        }

        public void Listen()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(_kafkaConfig).Build())
            {
                consumer.Subscribe(_kafkaTopics);
                
                var cts = new CancellationTokenSource();

                while(true)
                {
                    try
                    {
                        var message = consumer.Consume(cts.Token);
                        _alertStream.Publish(new Alert { Message = message.Value });
                        Console.WriteLine($"Consumed message: {message.Value}");
                    } catch (ConsumeException e)
                    {
                        Console.WriteLine($"Error occured: {e.Error.Reason}");
                    }
                }
            }
        }
    }
}
