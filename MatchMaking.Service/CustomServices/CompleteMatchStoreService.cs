using MatchMaking.Common.Types;
using MatchMaking.Shared.DataContracts;
using Microsoft.Extensions.Logging;

namespace MatchMaking.Service.CustomServices
{
    public class CompleteMatchStoreService : BaseService<MatchCompleteRequest, MatchCompleteResponse>
    {
        private readonly ICacheClient _cacheClient;

        public CompleteMatchStoreService(ICacheClient cache, ILogger<CompleteMatchStoreService> logger) : base(logger)
        {
            _cacheClient = cache;
        }

        protected override async Task Validate(MatchCompleteRequest request)
        {
            await base.Validate(request);
        }

        public override async Task<MatchCompleteResponse> Execute(MatchCompleteRequest request)
        {
            string matchUsersKey = $"{CommonConsts.UsersForMatchPrefixKey}_{request.MatchId}";
            await _cacheClient.AddToSet(matchUsersKey, request.UserIds);

            foreach (var userId in request.UserIds)
            {
                string userMatchKey = $"{CommonConsts.UserMatchedPrefixKey}_{userId}";
                await _cacheClient.SetByKey(userMatchKey, request.MatchId);
            }

            return new MatchCompleteResponse()
            {
                Success = true,
                Message = "Match stored successfully",
                MatchId = request.MatchId,
                UserIds = request.UserIds,
            };
        }
    }
}
