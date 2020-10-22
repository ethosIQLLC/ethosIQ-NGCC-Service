using System.Configuration;

namespace ethosIQ_NGCC_Shared.Configuration
{
    public class NGCCElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("IPAddress", IsRequired = true)]
        public string IPAddress
        {
            get { return (string)this["IPAddress"]; }
            set { this["IPAddress"] = value; }
        }

        [ConfigurationProperty("Port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["Port"]; }
            set { this["Port"] = value; }
        }

        [ConfigurationProperty("TenantID", IsRequired = true)]
        public string TenantID
        {
            get { return (string)this["TenantID"]; }
            set { this["TenantID"] = value; }
        }

        [ConfigurationProperty("Username", IsRequired = true)]
        public string Username
        {
            get { return (string)this["Username"]; }
            set { this["Username"] = value; }
        }

        [ConfigurationProperty("Password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }

        [ConfigurationProperty("RealTimeIPAddress", IsRequired = true)]
        public string RealtimeIPAddress
        {
            get { return (string)this["RealTimeIPAddress"]; }
            set { this["RealTimeIPAddress"] = value; }
        }

        [ConfigurationProperty("RealTimePort", IsRequired = true)]
        public int RealtimePort
        {
            get { return (int)this["RealTimePort"]; }
            set { this["RealTimePort"] = value; }
        }

        [ConfigurationProperty("RealTimeEnabled", IsRequired = true)]
        public bool RealtimeEnabled
        {
            get { return (bool)this["RealTimeEnabled"]; }
            set { this["RealTimeEnabled"] = value; }
        }
    }
}
