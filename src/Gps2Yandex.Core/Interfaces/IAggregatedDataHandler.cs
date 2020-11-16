using System.Collections.Generic;
using System.Threading.Tasks;

using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.Core.Interfaces
{
    public interface IAggregatedDataHandler
    {
        Task Execute(IEnumerable<AggregatedData> items);
    }
}

/*
т.е. есть агрегатор - соединяет все источники данных, 
есть кэш с вытеснением (по двум событиям - времени, повторяющейся записи) есть очередь отправки )
*/