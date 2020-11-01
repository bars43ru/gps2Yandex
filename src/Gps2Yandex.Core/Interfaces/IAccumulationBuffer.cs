using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.Core.Interfaces
{
    public interface IAccumulationBuffer
    {
        public void Add(AggregatedData data);
    }
}
