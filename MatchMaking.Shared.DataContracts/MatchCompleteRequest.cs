using MatchMaking.Common.Types;

namespace MatchMaking.Shared.DataContracts
{
    public record MatchCompleteRequest(string MatchId, string[] UserIds) : BaseRequest
    {
    }
}