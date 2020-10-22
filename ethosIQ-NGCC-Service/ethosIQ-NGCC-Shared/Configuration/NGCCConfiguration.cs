using ethosIQ_Configuration;
using System.Collections.Generic;
using System.Configuration;

namespace ethosIQ_NGCC_Shared.Configuration
{
    public class NGCCConfiguration
    {
        private static List<ethosIQSource> NGCCSources = new List<ethosIQSource>();

        public static List<ethosIQSource> GetConfiguration()
        {
            var CustomSection = ConfigurationManager.GetSection(NGCCConfigurationSection.SectionName) as NGCCConfigurationSection;

            if(CustomSection != null)
            {
                foreach(NGCCElement NGCCElement in CustomSection.NGCCSources)
                {
                    NGCCSource tempSource = new NGCCSource(NGCCElement.Name, NGCCElement.IPAddress, NGCCElement.Port, NGCCElement.TenantID, NGCCElement.Username, NGCCElement.Password, NGCCElement.RealtimeEnabled, NGCCElement.RealtimeIPAddress, NGCCElement.RealtimePort);

                    NGCCSources.Add(tempSource);
                }
            }

            return NGCCSources;
        }
    }
}
