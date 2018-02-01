using System.Configuration;

namespace Container.Config
{
    public class TenantsElementCollection : ConfigurationElementCollection
    {
        public TenantsElementCollection()
        {
            AddElementName = "tenant";
        }

        public TenantConfigurationElement this[int index]
        {
            get { return BaseGet(index) as TenantConfigurationElement; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TenantConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TenantConfigurationElement)element).Id;
        }
    }
}