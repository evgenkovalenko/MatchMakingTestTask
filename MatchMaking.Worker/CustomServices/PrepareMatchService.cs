
using MatchMaking.Common.Types;

using MatchMaking.Shared.DataContracts;

namespace MatchMaking.Worker.CustomServices
{
    public class PrepareMatchService : BaseService<MatchSearchRequest, BaseResponse>
    {
        private readonly IConfiguration configuration;
        private readonly ICacheClient cacheClient;
        private readonly IServiceProvider services;

        public PrepareMatchService(ILogger<PrepareMatchService> logger,
                                    IConfiguration config,
                                    ICacheClient cache,
                                    IServiceProvider services
                                    ) : base(logger)
        {
            configuration = config;
            cacheClient = cache;
            this.services = services;
        }

        public override async Task<BaseResponse> Execute(MatchSearchRequest request)
        {
            bool added = await cacheClient.AddToSet(CommonConsts.CurrentMatch, request.UserId);
            if (added)
            {
                string[] users = await cacheClient.GetSetMembersByKey(CommonConsts.CurrentMatch);
                var str = configuration.GetSection("AppSettings")?["UsersCountPerMatch"];
                int playersPerMatch = CommonConsts.DefaultPlayersPerMatch;
                if (!string.IsNullOrEmpty(str) && Int32.TryParse(str, out int playersPerMatchConfig))
                {
                    playersPerMatch = playersPerMatchConfig;
                }

                if (users.Length == playersPerMatch)
                {
                    await cacheClient.DeleteKey(CommonConsts.CurrentMatch);
                    await SendMatchCompleteMessage(users);
                }
                else if(users.Length > playersPerMatch)
                {
                    // TODO: handle situation by sending only the first playersPerMatch users
                    await SendMatchCompleteMessage(users);
                }
            }

            return new BaseResponse
            {
                Success = true
            };
        }

        private async Task SendMatchCompleteMessage(string[] users)
        {
            var service = services.GetRequiredService<BaseService<MatchCompleteRequest, MatchCompleteResponse>>();

            string matchId = Guid.NewGuid().ToString();

            var _ = await service.Handle(new MatchCompleteRequest(matchId, users));
        }
    }
}
