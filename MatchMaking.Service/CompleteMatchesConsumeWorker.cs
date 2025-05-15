using MatchMaking.Common.Types;

using MatchMaking.Shared.DataContracts;
using System.Text.Json;

namespace MatchMaking.Service
{
    public class CompleteMatchesConsumeWorker(ILogger<CompleteMatchesConsumeWorker> logger,
                                        IServiceProvider services,
                                        IMessagesConsumer messagesConsumer) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("CompleteMatchesConsumeWorker Started");

            await Task.Delay(3000, stoppingToken); // Allow application to start before consuming messages

            await messagesConsumer.StartConsuming(CommonConsts.MatchMakingCompleteTopic, ProcessConsumedMessages, stoppingToken);

            logger.LogInformation("CompleteMatchesConsumeWorker Stopping");
        }

        private async Task<bool> ProcessConsumedMessages(Tuple<string, string> message)
        {
            string messageStr = message.Item2;

            MatchCompleteRequest matchCompleteRequest = JsonSerializer.Deserialize<MatchCompleteRequest>(messageStr);

            var service = services.GetRequiredService<BaseService<MatchCompleteRequest, MatchCompleteResponse>>();
            var response = await service.Handle(matchCompleteRequest);

            return response.Success;
        }
    }
}