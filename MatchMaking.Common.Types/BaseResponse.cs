using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaking.Common.Types
{
    public record BaseResponse()
    {
        public bool Success { get; set; } = true;

        public string Message { get; set; } = string.Empty;

        public double ExecutionTime { get; set; } = 0.0;
    }
}
