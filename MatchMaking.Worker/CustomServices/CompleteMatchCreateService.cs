using MatchMaking.Common.Types;
using MatchMaking.Shared.DataContracts;

using System.Text.Json;

namespace MatchMaking.Worker.CustomServices
{
    public class CompleteMatchCreateService : BaseService<MatchCompleteRequest, MatchCompleteResponse>
    {
        private readonly IMessageProducer messageProducer;

        public CompleteMatchCreateService(IMessageProducer producer, ILogger<CompleteMatchCreateService> logger) : base(logger)
        {
            messageProducer = producer;
        }

        protected override async Task Validate(MatchCompleteRequest request)
        {
            await base.Validate(request);
        }

        public override async Task<MatchCompleteResponse> Execute(MatchCompleteRequest request)
        {
            string matchMessage = JsonSerializer.Serialize(request);

            await messageProducer.ProduceAsync(CommonConsts.MatchMakingCompleteTopic, request.MatchId, matchMessage);

            return new MatchCompleteResponse
            {
                Success = true,
                Message = "Match completed"
            };
        }
    }
}