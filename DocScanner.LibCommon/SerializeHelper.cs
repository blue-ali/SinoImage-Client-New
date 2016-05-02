using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace DocScanner.LibCommon
{
    public static class SerializeHelper
    {
        // Fields
        private static Dictionary<Type, XmlSerializer> _cache;
        private static XmlSerializerNamespaces _defaultNamespace = new XmlSerializerNamespaces();

        // Methods
        static SerializeHelper()
        {
            _defaultNamespace.Add(string.Empty, string.Empty);
            _cache = new Dictionary<Type, XmlSerializer>();
        }

        public static T DeSerializeFromXML<T>(string fname) where T : class
        {
            T local2;
            if (!File.Exists(fname))
            {
                return default(T);
            }
            XmlSerializer serializer = GetSerializer<T>();
            using (TextReader reader = new StreamReader(fname))
            {
                object obj2 = null;
                try
                {
                    obj2 = serializer.Deserialize(reader);
                }
                catch (Exception)
                {
                }
                reader.Close();
                try
                {
                    if (obj2 is T)
                    {
                        return (T)obj2;
                    }
                    local2 = default(T);
                }
                catch
                {
                    local2 = default(T);
                }
            }
            return local2;
        }

        public static T DS<T>(byte[] bytes)
        {
            object obj2 = null;
            MemoryStream serializationStream = new MemoryStream(bytes)
            {
                Position = 0L
            };
            obj2 = new BinaryFormatter().Deserialize(serializationStream);
            serializationStream.Close();
            return (T)obj2;
        }

        public static byte[] ES(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            MemoryStream serializationStream = new MemoryStream();
            new BinaryFormatter().Serialize(serializationStream, obj);
            serializationStream.Position = 0L;
            byte[] buffer = new byte[serializationStream.Length];
            serializationStream.Read(buffer, 0, buffer.Length);
            serializationStream.Close();
            return buffer;
        }

        public static XmlSerializer GetSerializer<T>()
        {
            Type key = typeof(T);
            if (!_cache.ContainsKey(key))
            {
                Type[] types = new Type[] { key };
                _cache[key] = XmlSerializer.FromTypes(types)[0];
            }
            return _cache[key];
        }

        public static void SerializeToXML<T>(object obj, string fname) where T : class
        {
            if (obj != null)
            {
                XmlSerializer serializer = GetSerializer<T>();
                using (TextWriter writer = new StreamWriter(fname))
                {
                    serializer.Serialize(writer, obj);
                    writer.Close();
                }
            }
        }

        public static string ToXMLString<T>(this object obj) where T : class
        {
            if (obj == null)
            {
                return string.Empty;
            }
            XmlSerializer serializer = GetSerializer<T>();
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize((TextWriter)writer, obj);
                return writer.ToString();
            }
        }

        public static T XmlDeserialize<T>(this string xml)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                object obj2 = GetSerializer<T>().Deserialize(stream);
                return ((obj2 == null) ? default(T) : ((T)obj2));
            }
        }

        public static string XmlSerialize<T>(this T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                GetSerializer<T>().Serialize(stream, obj, _defaultNamespace);
                return Encoding.UTF8.GetString(stream.GetBuffer());
            }
        }
    }

}