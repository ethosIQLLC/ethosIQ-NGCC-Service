using System.Collections.Generic;
using System.Xml.Serialization;

namespace ethosIQ_NGCC_Shared
{
    [XmlRoot(ElementName = "G")]
    public class Group
    {
        [XmlAttribute(AttributeName = "id")]
        public string GroupId { get; set; }
        [XmlAttribute(AttributeName = "n")]
        public string GroupName { get; set; }
    }

    [XmlRoot(ElementName = "A")]
    public class Agent
    {
        [XmlElement(ElementName = "G")]
        public Group Group { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string TouchpointID { get; set; }
        [XmlAttribute(AttributeName = "fn")]
        public string FirstName { get; set; }
        [XmlAttribute(AttributeName = "ln")]
        public string LastName { get; set; }
        [XmlAttribute(AttributeName = "desc")]
        public string Description { get; set; }
        [XmlAttribute(AttributeName = "un")]
        public string Username { get; set; }
        [XmlAttribute(AttributeName = "sk")]
        public string Skills { get; set; }
        [XmlAttribute(AttributeName = "tm")]
        public string Teams { get; set; }
        [XmlAttribute(AttributeName = "bic")]
        public string BlockInternationalCalls { get; set; }
        [XmlAttribute(AttributeName = "bldc")]
        public string BlockLongDistantCalls { get; set; }
        [XmlAttribute(AttributeName = "blc")]
        public string BlockLocalCalls { get; set; }
        [XmlAttribute(AttributeName = "bc")]
        public string BlockCodes { get; set; }
        [XmlAttribute(AttributeName = "isag")]
        public string IsAgent { get; set; }
        [XmlAttribute(AttributeName = "isad")]
        public string IsAdministrator { get; set; }
        [XmlAttribute(AttributeName = "issu")]
        public string IsSupervisor { get; set; }
    }

    [XmlRoot(ElementName = "AConfigs")]
    public class AConfigs
    {
        [XmlElement(ElementName = "A")]
        public List<Agent> A { get; set; }
        [XmlAttribute(AttributeName = "ts")]
        public string TimeStamp { get; set; }
        [XmlAttribute(AttributeName = "tzo")]
        public string TimezoneOffset { get; set; }
        [XmlAttribute(AttributeName = "al")]
        public string AgentLimitation { get; set; }
        [XmlAttribute(AttributeName = "smtpserver")]
        public string Smtpserver { get; set; }
        [XmlAttribute(AttributeName = "loginid")]
        public string Loginid { get; set; }
    }
}
