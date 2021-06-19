using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Gps2Yandex.Yandex.Entities
{
    /// <summary>
    /// Категория GPS-сигнала:
    /// ⦁	Slow - "медленный" (устанавливается для треков общественного транспорта);
    /// ⦁	Normal - обычный.
    /// </summary>
    public enum GpsSignal 
    {
        [XmlEnum(Name = "s")]
        Slow,
        [XmlEnum(Name = "n")]
        Normal
    }

    /// <summary>
    /// Тип общественного транспортного средства: 
    /// ⦁	Bus - автобус;
    /// ⦁	Trolleybus - троллейбус;
    /// ⦁	Tramway - трамвай;
    /// ⦁	Minibus - маршрутное такси.
    /// </summary>
    public enum VehicleType
    {
        [XmlEnum(Name = "bus")]
        Bus,
        [XmlEnum(Name = "trolleybus")]
        Trolleybus,
        [XmlEnum(Name = "tramway")]
        Tramway,
        [XmlEnum(Name = "minibus")]
        Minibus
    }

    /// <summary>
    /// Данные о транспортном средстве и маршруте по которому он движется
    /// </summary>
    [Serializable]
    public struct Track
    {
        /// <summary>
        /// Идентификатор движущегося объекта (транспортного средства). 
        //  Длина идентификатора не должна превышать 32 символа и содержать только символы латинского алфавита и цифры.
        //  uuid=0d63b6deacb91b00e46194fac325b72a
        /// </summary>
        [XmlAttribute(AttributeName = "uuid")]
        public string Uuid { get; init; }

        /// <summary>
        /// Категория GPS-сигнала:
        /// ⦁	s - "медленный" (устанавливается для треков общественного транспорта);
        /// ⦁	n - обычный.
        /// category=s
        /// </summary>
        [XmlAttribute(AttributeName = "category")]
        public GpsSignal Category { get; init; }

        /// <summary>
        /// Идентификатор маршрута. 
        /// route=190Б
        /// </summary>
        [XmlAttribute(AttributeName = "route")]
        public string Route { get; init; }

        /// <summary>
        /// Тип общественного транспортного средства: 
        /// ⦁	bus - автобус;
        /// ⦁	trolleybus - троллейбус;
        /// ⦁	tramway - трамвай;
        /// ⦁	minibus - маршрутное такси.
        /// vehicle_type=minibus
        /// </summary>
        [XmlAttribute(AttributeName = "vehicle_type")]
        public VehicleType VehicleType { get; init; }

        /// <summary>
        /// Данные об последнем актуальном местоположении данного транспортного средства
        /// </summary>
        [XmlElement(ElementName = "point")]
        public Point Point { get; init; }

        public Track(string uuid, string route, Point point, VehicleType vehicleType = VehicleType.Bus, GpsSignal category = GpsSignal.Slow)
        {
            Uuid = uuid;
            Route = route;
            Point = point;
            VehicleType = vehicleType;
            Category = category;
        }
    }
}
