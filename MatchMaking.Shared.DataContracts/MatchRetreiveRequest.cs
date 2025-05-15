using MatchMaking.Common.Types;

namespace MatchMaking.Shared.DataContracts
{
    public record MatchRetreiveRequest(string UserId): BaseRequest
    {
    }
}