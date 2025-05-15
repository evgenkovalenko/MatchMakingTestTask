using Confluent.Kafka;

using MatchMaking.Common.Types;
using Microsoft.Extensions.Configuration;


namespace MatchMaking.Common.Kafka
{
    public class KafkaConsumer(IConfiguration configuration) : IMessagesConsumer
    {
        public Task Initialize(string[] args)
        {
            return Task.CompletedTask;
        }

        public async Task<bool> StartConsuming(string topic, Func<Tuple<string, string>, Task<bool>> processFunc, CancellationToken stoppingToken)
        {
            ConsumerConfig config = new ConsumerConfig
            {
                BootstrapServers = configuration.GetSection("Kafka")["BootstrapServers"],
                GroupId = configuration.GetSection("Kafka")["ConsumerGroup"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(topic);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);

                    Tuple<string, string> message = new Tuple<string, string>(consumeResult.Message.Key, consumeResult.Message.Value);

                    bool processed = await processFunc(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while consuming messages.{ex.GetFullMessage()}");
                consumer.Close();
            }

            return true;
        }
    }
}
