using System;
using System.Xml.Serialization;

namespace Gps2Yandex.Yandex.Entities
{

    /// <summary>
    /// Данные о транспортном средстве и маршруте по которому он движется
    /// </summary>
    [Serializable]
    public class Track
    {
        /// <summary>
        /// Идентификатор движущегося объекта (транспортного средства). 
        //  Длина идентификатора не должна превышать 32 символа и содержать только символы латинского алфавита и цифры.
        //  uuid=0d63b6deacb91b00e46194fac325b72a
        /// </summary>
        [XmlAttribute(AttributeName = "uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// Категория GPS-сигнала:
        /// ⦁	s - "медленный" (устанавливается для треков общественного транспорта);
        /// ⦁	n - обычный.
        /// category=s
        /// </summary>
        [XmlAttribute(AttributeName = "category")]
        public char Category { get; set; } = 's';

        /// <summary>
        /// Идентификатор маршрута. 
        /// route=190Б
        /// </summary>
        [XmlAttribute(AttributeName = "route")]
        public string Route { get; set; }

        /// <summary>
        /// Тип общественного транспортного средства: 
        /// ⦁	bus - автобус;
        /// ⦁	trolleybus - троллейбус;
        /// ⦁	tramway - трамвай;
        /// ⦁	minibus - маршрутное такси.
        /// vehicle_type=minibus
        /// </summary>
        [XmlAttribute(AttributeName = "vehicle_type")]
        public string VehicleType { get; set; } = "bus";

        /// <summary>
        /// Данные об последнем актуальном местоположении данного транспортного средства
        /// </summary>
        [XmlElement(ElementName = "point")]
        public Point Point { get; set; }
    }

}
