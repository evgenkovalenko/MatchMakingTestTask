using MatchMaking.Common.Types;

namespace MatchMaking.Shared.DataContracts
{
    public record MatchSearchRequest(string UserId): BaseRequest
    {
    }
}
