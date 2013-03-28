using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SimulationManager.Worker.Helper
{
    public class Deployment : IXmlSerializable
    {
        public Deployment() { }
        public String DeploymentName { get; set; }
        public Uri packageUri { get; set; }
        public String Configuration { get; set; }
        public String Label { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader xmlReader)
        {
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();

            xmlReader.ReadStartElement();

            DeploymentName = xmlReader.ReadElementContentAsString();
            String deploymentSlot = xmlReader.ReadElementContentAsString();
            String privateId = xmlReader.ReadElementContentAsString();

            String status = xmlReader.ReadElementContentAsString();

            String base64Label = xmlReader.ReadElementContentAsString();
            Byte[] labelData = System.Convert.FromBase64String(base64Label);
            Label = asciiEncoding.GetString(labelData);

            String packageUriString = xmlReader.ReadElementContentAsString();

            String base64Configuration =    xmlReader.ReadElementContentAsString();
          //  Byte[] configurationData =System.Convert.FromBase64String(base64Configuration);
            Configuration = base64Configuration;//asciiEncoding.GetString(configurationData);
        }

        public void WriteXml(XmlWriter xmlWriter)
        {
            Byte[] configurationBytes = System.Text.Encoding.UTF8.GetBytes(Configuration);
            Byte[] labelBytes = System.Text.Encoding.UTF8.GetBytes(Label);

            xmlWriter.WriteElementString("Name", DeploymentName);
            xmlWriter.WriteElementString("PackageUrl",
            packageUri.AbsoluteUri);
            xmlWriter.WriteStartElement("Label");
            xmlWriter.WriteBase64(labelBytes, 0, labelBytes.Count<Byte>());
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Configuration");
            xmlWriter.WriteBase64(configurationBytes, 0,
            configurationBytes.Count<Byte>());
            xmlWriter.WriteEndElement();
        }
    }
}
