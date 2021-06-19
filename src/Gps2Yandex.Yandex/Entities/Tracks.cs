using System;
using System.Xml.Serialization;

namespace Gps2Yandex.Yandex.Entities
{
    /// <summary>
    /// Пакет передаваемых данных
    /// </summary>
    [Serializable, XmlRoot("tracks")]
    public struct Tracks
    {
        /// <summary>
        /// Идентификатор участника программы.
        /// Длина идентификатора не должна превышать 32 символа и содержать только символы латинского алфавита и цифры.
        /// </summary>
        [XmlAttribute(AttributeName = "clid")]
        public string Clid { get; init; }

        /// <summary>
        /// Перечень машин по которым будем высылать информацию
        /// </summary>
        [XmlElement(ElementName = "track")]
        public Track[] Items { get; init; }

        public Tracks(string clid, params Track[] items)
        {
            Clid = clid;
            Items = items;
        }
    }

}
