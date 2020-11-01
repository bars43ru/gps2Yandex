using System.Collections.Concurrent;

using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.Core.Services
{
    class SendingQueue
    {
        ConcurrentQueue<AggregatedData[]> Queue { get; } = new ConcurrentQueue<AggregatedData[]>();
    }
}
