using Confluent.Kafka;
using Confluent.Kafka.Admin;
using MatchMaking.Common.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace MatchMaking.Common.Kafka
{
    public class KafkaProducer(IConfiguration configuration) : IMessageProducer
    {
        private IProducer<string, string> _prd;

        private bool isInitialized = false;

        private async Task<IProducer<string, string>> GetProducer()
        {
            if (_prd == null)
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = configuration.GetSection("Kafka")["BootstrapServers"],
                    ClientId = Dns.GetHostName()
                };

                _prd = new ProducerBuilder<string, string>(config).Build();
            }

            return _prd;
        }

        public async Task<bool> ProduceAsync(string topic, string key, string val)
        {
            if(!isInitialized)
            {
                await Initialize(null);
            }

            var producer = await GetProducer();
            var result = await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = val });

            if (result.Status == PersistenceStatus.Persisted)
            {
                return true;
            }
            else
            {
                throw new Exception($"Failed to produce message: {result.Message}");
            }
        }

        private async Task ConfigureTopic(string topicName, ProducerConfig conf)
        {
            using (var adminClient = new AdminClientBuilder(new AdminClientConfig(conf)).Build())
            {
                var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(10));

                var topic = meta.Topics.SingleOrDefault(t => t.Topic == topicName);

                if (topic == null)
                {
                    await adminClient.CreateTopicsAsync(new[]
                    {
                        new TopicSpecification
                        {
                            Name = topicName, 
                            ReplicationFactor = 1, 
                            NumPartitions = 3 
                        }
                    });
                }
            }
        }

        public async Task Initialize(string[] args)
        {
            string topicsStr = configuration.GetSection("Kafka")["Topics"];
            string[] topics = topicsStr.SplitByComma();

            var config = new ProducerConfig
            {
                BootstrapServers = configuration.GetSection("Kafka")["BootstrapServers"],
                ClientId = Dns.GetHostName()
            };

            foreach (var topic in topics)
            {
                await ConfigureTopic(topic, config);
            }

            isInitialized = true;
        }
    }
}
