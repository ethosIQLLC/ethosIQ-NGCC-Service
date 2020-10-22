using System.Configuration;

namespace ethosIQ_NGCC_Shared.Configuration
{
    [ConfigurationCollection(typeof (NGCCElement))]
    public class NGCCCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new NGCCElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NGCCElement)element).Name;
        }
    }
}
