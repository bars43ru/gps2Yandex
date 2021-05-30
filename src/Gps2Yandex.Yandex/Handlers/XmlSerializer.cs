using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gps2Yandex.Yandex.Handlers
{
    public static class XmlSerializer
    {
        public static string Serialize<T>(T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using var stringWriter = new StringWriterUtf8();
            serializer.Serialize(stringWriter, value, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            return stringWriter.ToString();
        }

        private class StringWriterUtf8 : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
