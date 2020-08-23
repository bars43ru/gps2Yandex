using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gps2Yandex.Yandex.Models
{
    /// <summary>
    /// Пакет передаваемых данных
    /// </summary>
    [Serializable, XmlRoot("tracks")]
    public class Tracks
    {
        /// <summary>
        /// Идентификатор участника программы.
        /// Длина идентификатора не должна превышать 32 символа и содержать только символы латинского алфавита и цифры.
        /// </summary>
        [XmlAttribute(AttributeName = "clid")]
        public string Clid { get; set; }       
        
        /// <summary>
        /// Перечень машин по которым будем высылать информацию
        /// </summary>
        [XmlElement(ElementName = "track")]
        public List<Track> Items { get; set; }
    }

}
