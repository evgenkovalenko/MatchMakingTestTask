using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaking.Common.Types
{
    public interface IMessagesConsumer
    {
        Task Initialize(string[] args);

        Task<bool> StartConsuming(string topic, Func<Tuple<string, string>, Task<bool>> processFunc, CancellationToken stoppingToken);

    }
}
