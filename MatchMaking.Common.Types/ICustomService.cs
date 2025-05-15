using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaking.Common.Types
{
    public interface ICustomService<TRequest, TResponse> where TRequest : BaseRequest where TResponse : BaseResponse, new()
    {
        Task<TResponse> Handle(TRequest request);
    }
}
