using System;
using System.Xml.Serialization;

namespace Gps2Yandex.Yandex.Entities
{

    /// <summary>
    /// Данные о местоположении общественного транспорта (только для треков общественного транспорта)
    /// </summary>
    [Serializable]
    public struct Point
    {
        /// <summary>
        /// Долгота точки в градусах. В качестве десятичного разделителя используется точка.
        /// longitude=37.620070
        /// </summary>
        [XmlAttribute(AttributeName = "latitude")]
        public double Latitude { get; init; }

        /// <summary>
        /// Широта точки в градусах. В качестве десятичного разделителя используется точка.
        /// latitude=55.753630
        /// </summary>
        [XmlAttribute(AttributeName = "longitude")]
        public double Longitude { get; init; }

        /// <summary>
        /// Мгновенная скорость транспортного средства, полученная от приемника GPS, км/ч.
        /// avg_speed=53
        /// </summary>
        [XmlAttribute(AttributeName = "avg_speed")]
        public uint AvgSpeed { get; init; }

        /// <summary>
        /// Направление движения в градусах (направление на север - 0 градусов). Диапазон значений 0-360.
        /// direction=242
        /// </summary>
        [XmlAttribute(AttributeName = "direction")]
        public uint Direction { get; init; }

        /// <summary>
        /// Дата и время получения координат точки от GPS-приемника (по Гринвичу). Формат: ДДММГГГГ:ччммсс 
        /// time=10012009:142045
        /// </summary>
        /// TODO Need go to DateTime and serialization sample https://www.codeproject.com/Tips/831949/Serialize-System-TimeSpan-to-XML
        [XmlAttribute(AttributeName = "time")]
        public string Time { get; init; }
    }
}
