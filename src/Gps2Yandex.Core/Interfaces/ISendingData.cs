using System.Collections.Generic;
using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.Core.Interfaces
{
    public interface ISendingData
    {
        void Send(IEnumerable<AggregatedData> items);
    }
}


/*
т.е. есть агрегатор - соединяет все источники данных, 
есть кэш с вытеснением (по двум событиям - времени, повторяющейся записи) есть очередь отправки )
*/