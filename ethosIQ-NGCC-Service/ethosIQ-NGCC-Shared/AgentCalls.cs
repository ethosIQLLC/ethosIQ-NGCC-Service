using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ethosIQ_NGCC_Shared
{
    [XmlRoot(ElementName = "A")]
    public class AgentActiveCall
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }//
        [XmlAttribute(AttributeName = "tp")]
        public string Tp { get; set; }//
        [XmlAttribute(AttributeName = "tl")]
        public string Tl { get; set; }//
        [XmlAttribute(AttributeName = "as")]
        public string As { get; set; }//
        [XmlAttribute(AttributeName = "cc")]
        public string Cc { get; set; }//
        [XmlAttribute(AttributeName = "all")]
        public string All { get; set; }//
        [XmlAttribute(AttributeName = "aics")]
        public string Aics { get; set; }//
        [XmlAttribute(AttributeName = "cid")]
        public string Cid { get; set; }//
        [XmlAttribute(AttributeName = "gid")]
        public string Gid { get; set; }//
        [XmlAttribute(AttributeName = "rc")]
        public string Rc { get; set; }
        [XmlAttribute(AttributeName = "dir")]
        public string Dir { get; set; }
        [XmlAttribute(AttributeName = "cct")]
        public string Cct { get; set; }//
        [XmlAttribute(AttributeName = "cq")]
        public string Cq { get; set; }//
        [XmlAttribute(AttributeName = "inatt")]
        public string Inatt { get; set; }//
        [XmlAttribute(AttributeName = "outatt")]
        public string Outatt { get; set; }//
        [XmlAttribute(AttributeName = "a2aatt")]
        public string A2aatt { get; set; }//
        [XmlAttribute(AttributeName = "tatt")]
        public string Tatt { get; set; }//
    }

    [XmlRoot(ElementName = "ACalls")]
    public class ACalls
    {
        [XmlElement(ElementName = "A")]
        public List<AgentActiveCall> A { get; set; }
        [XmlAttribute(AttributeName = "ts")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "tzo")]
        public string TimezoneOffset { get; set; }
        [XmlAttribute(AttributeName = "al")]
        public string AgentLimitation { get; set; }
        [XmlAttribute(AttributeName = "cts")]
        public string LastConfigurationUpdateTimeStamp { get; set; }//-----------

    }
}
