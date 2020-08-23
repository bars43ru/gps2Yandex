using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Gps2Yandex.Yandex.Extensions
{
    internal static class XmlSerializerExtensions
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriterUtf8();
                xmlserializer.Serialize(stringWriter, value, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private class StringWriterUtf8 : StringWriter
        {
            public override Encoding Encoding
            {
                get
                {
                    return Encoding.UTF8;
                }
            }
        }
    }
}
