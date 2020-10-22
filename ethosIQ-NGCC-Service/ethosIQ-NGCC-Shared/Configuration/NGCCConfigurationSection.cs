using System.Configuration;

namespace ethosIQ_NGCC_Shared.Configuration
{
    public class NGCCConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "NGCCConfiguration";
        public const string CollectionName = "NGCCSources";

        [ConfigurationProperty(CollectionName)]
        [ConfigurationCollection(typeof(NGCCCollection), AddItemName = "NGCCSource")]
        public NGCCCollection NGCCSources { get { return (NGCCCollection)base[CollectionName]; } }
    }
}
