using MatchMaking.Common.Types;

namespace MatchMaking.Shared.DataContracts
{
    public record MatchCompleteResponse : BaseResponse
    {
        public string MatchId { get; set; } = string.Empty;

        public string[] UserIds { get; set; }
    }
}