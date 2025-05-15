using MatchMaking.Common.Types;
using MatchMaking.Shared.DataContracts;

namespace MatchMaking.Service.CustomServices
{
    public class MatchRetreiveService : BaseService<MatchRetreiveRequest, MatchCompleteResponse>
    {
        private readonly ICacheClient _cacheClient;

        public MatchRetreiveService(ICacheClient cache, ILogger<MatchRetreiveService> logger) : base(logger)
        {
            _cacheClient = cache;
        }

        protected override async Task Validate(MatchRetreiveRequest request)
        {
            await base.Validate(request);

            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                throw new ApplicationException("UserId cannot be null or empty");
            }
        }

        public override async Task<MatchCompleteResponse> Execute(MatchRetreiveRequest request)
        {
            string userMatchKey = $"{CommonConsts.UserMatchedPrefixKey}_{request.UserId}";
            var matchId  = await _cacheClient.GetByKey(userMatchKey);
            
            if (string.IsNullOrEmpty(matchId))
            {
                throw new ApplicationException("No match found");
            }

            string matchUsersKey = $"{CommonConsts.UsersForMatchPrefixKey}_{matchId}";
            string[] users = await _cacheClient.GetSetMembersByKey(matchUsersKey);

            if (users == null || users.Length == 0)
            {
                throw new ApplicationException("Users not found");
            }
            
            return new MatchCompleteResponse()
            {
                Success = true,
                Message = "Match found",
                MatchId = matchId,
                UserIds = users,
            };
        }
    
    }
}
