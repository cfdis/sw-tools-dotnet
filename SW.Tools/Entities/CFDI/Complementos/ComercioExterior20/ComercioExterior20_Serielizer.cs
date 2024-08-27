using SW.Tools.Cfdi.Complementos.ComercioExterior20;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace SW.Tools.Helpers
{
    public partial class SerializerCE
    {
        public static string SerializeDocument(ComercioExterior comercioExterior)
        {
            MemoryStream stream = null;
            TextWriter writer = null;
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("cce20", "http://www.sat.gob.mx/ComercioExterior20");

                stream = new MemoryStream();
                writer = new StreamWriter(stream, encoding);

                XmlSerializer serializer = new XmlSerializer(typeof(ComercioExterior));
                serializer.Serialize(writer, comercioExterior, ns);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                string xml = encoding.GetString(arr).Trim();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                return doc.OuterXml;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (stream != null) stream.Close();
                if (writer != null) writer.Close();
            }
        }
    }
}
