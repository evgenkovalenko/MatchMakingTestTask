using MatchMaking.Common.Kafka;
using MatchMaking.Common.Redis;
using MatchMaking.Common.Types;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MatchMaking.Common.App
{
    public static class Extensions
    {
        public static IServiceCollection ApplicationDefaultRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICacheClient, RedisCacheClient>();
            services.AddSingleton<IMessageProducer, KafkaProducer>();
            services.AddSingleton<IMessagesConsumer, KafkaConsumer>();

            return services;
        }

        public static WebApplication WarmUp(this WebApplication application, IServiceProvider services)
        {
            var producer = services.GetRequiredService<IMessageProducer>();
            var cache = services.GetRequiredService<ICacheClient>();

            int attempt = 1;
            int maxRetries = 5;

            while (true)
            {
                try
                {
                    cache.Init();
                                        
                    var t = producer.Initialize(args: null);
                    t.GetAwaiter().GetResult();

                    break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error. WarmUp application: {ex.GetFullMessage()}");
                    Thread.Sleep(3000);
                    
                    if(attempt++ > maxRetries)
                    {
                        throw new ApplicationException($"Failed to WarmUp application after {maxRetries} attempts");
                    }
                }
            }

            return application;
        }

    }
}
