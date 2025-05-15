using MatchMaking.Common.Types;
using MatchMaking.Shared.DataContracts;

namespace MatchMaking.Service.CustomServices
{
    public class MatchSearchService : BaseService<MatchSearchRequest, MatchSearchResponse>
    {
        private readonly ICacheClient _cacheClient;
        
        private readonly IMessageProducer _kafkaProducer;

        public MatchSearchService(ICacheClient cache, IMessageProducer kafka, ILogger<MatchSearchService> logger) : base(logger)
        {
            _cacheClient = cache;
            _kafkaProducer = kafka;
        }

        protected override async Task Validate(MatchSearchRequest request)
        {
            await base.Validate(request);

            bool allowed = await _cacheClient.SetByKey(CommonConsts.RateLimitKey, "_", TimeSpan.FromMilliseconds(CommonConsts.RateLimitMs));
            if (!allowed)
            {
                throw new ApplicationException("Rate limit exceeded");
            }
        }

        public override async Task<MatchSearchResponse> Execute(MatchSearchRequest request)
        {
            await _kafkaProducer.ProduceAsync(CommonConsts.MatchMakingRequestTopic, request.UserId, request.UserId);

            var response = new MatchSearchResponse()
            {
                Success = true,
                Message = "Matchmaking request sent",
            };

            return response;
        }
    }
}
