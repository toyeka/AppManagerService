using System.Configuration;

namespace Container.Config
{
    public class TenantsConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "templateTenants";
        const string TenantsCollectionPropertyName = "tenants";

        [ConfigurationProperty(TenantsCollectionPropertyName, IsRequired = true)]
        public TenantsElementCollection Tenants
        {
            get
            {
                return this[TenantsCollectionPropertyName] as TenantsElementCollection;
            }
        }
    }
}