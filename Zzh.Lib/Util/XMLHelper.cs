using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Zzh.Lib.Util
{
    public static class XMLHelper
    {
        #region 反序列化
        public static object Deserialize<T>(string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static object Deserialize<T>(Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(typeof(T));
            return xmldes.Deserialize(stream);
        }
        #endregion
        #region 序列化
        /// <summary>
        /// 生成一个无限定名、无声明的XML字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serialObject"></param>
        /// <returns></returns>
        public static string XmlSerializer<T>(T serialObject) where T : class
        {
            var emptyNamepsaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(serialObject.GetType());
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Encoding = Encoding.UTF8;
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, serialObject, emptyNamepsaces);
                return stream.ToString();
            }
        }
        public static string Serializer<T>(T obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(typeof(T));
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        public static XDocument Convert(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);//强制调整指针位置
            using (XmlReader xr = XmlReader.Create(stream))
            {
                return XDocument.Load(xr);
            }
        }
        #endregion
    }
}
