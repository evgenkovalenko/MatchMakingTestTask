using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaking.Common.Types
{
    public interface IMessageProducer
    {
        Task Initialize(string[] args);
                
        Task<bool> ProduceAsync(string topic, string key, string val);
    }
}
