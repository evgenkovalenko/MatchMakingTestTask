using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaking.Common.Types
{
    public interface ICacheClient
    {
        Task Init();
                
        Task<bool> AddToSet(string setKey, string member);

        Task<long> AddToSet(string setKey, string[] members);

        Task<string[]> GetSetMembersByKey(string setKey);

        Task<bool> SetByKey(string key, string val);

        Task<bool> SetByKey(string key, string val, TimeSpan? ttl);

        Task<string> GetByKey(string key);

        Task DeleteKey(string key);
    }
}
