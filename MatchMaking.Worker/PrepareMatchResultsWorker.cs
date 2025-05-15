
using MatchMaking.Common.Kafka;
using MatchMaking.Common.Types;
using MatchMaking.Shared.DataContracts;
using MatchMaking.Worker.CustomServices;

public class PrepareMatchResultsWorker(ILogger<PrepareMatchResultsWorker> logger, 
                                        IServiceProvider services, 
                                        IMessagesConsumer messagesConsumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("PrepareMatchResultsWorker Started");

        await Task.Delay(3000, stoppingToken); // Allow application to start before consuming messages

        await messagesConsumer.StartConsuming( CommonConsts.MatchMakingRequestTopic, ProcessConsumedMessages, stoppingToken); 

        logger.LogInformation("PrepareMatchResultsWorker Stopping");
    }

    private async Task<bool> ProcessConsumedMessages(Tuple<string, string> message)
    {
        string userId = message.Item2;

        var service = services.GetRequiredService<BaseService<MatchSearchRequest, BaseResponse>>();
        var response = await service.Handle(new MatchSearchRequest(userId));
        
        return response.Success;
    }
}
