namespace MatchMaking.Common.Types
{
    public static class CommonConsts
    {
        public const int RateLimitMs = 100;

        public const int DefaultPlayersPerMatch = 3;


        //-------------------Redis Keys-------------------
        public const string RateLimitKey = "RateLimit";

        public const string UserMatchedPrefixKey = "UserMatchId";
        public const string UsersForMatchPrefixKey = "UsersForMatch";

        public const string CurrentMatch = "CurrentMatch";


        // -------------------Topics-------------------

        public const string MatchMakingRequestTopic = "matchmaking.request";

        public const string MatchMakingCompleteTopic = "matchmaking.complete";
    }
}
