using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Container.Config
{
    public class TenantConfigurationElement : ConfigurationElement
    {
        private const string IdPropertyName = "id";
        private const string UnityConfigPathPropertyName = "unityConfig";
//        private const string ConnectionStringPropertyName = "connectionString";

        [ConfigurationProperty(IdPropertyName, IsRequired = true)]
        public string Id
        {
            get { return this[IdPropertyName] as string; }
        }

        [ConfigurationProperty(UnityConfigPathPropertyName, IsRequired = true)]
        public string UnityConfigPath
        {
            get { return this[UnityConfigPathPropertyName] as string; }
        }

//        [ConfigurationProperty(ConnectionStringPropertyName, IsRequired = true)]
//        public string ConnectionString
//        {
//            get { return this[ConnectionStringPropertyName] as string; }
//        }
    }

    
    public class ConfigurationSectionReader<T> where T : ConfigurationSection, new()
    {
        public T GetSection(string fullConfigPath)
        {
            using (var stream = new StreamReader(fullConfigPath, Encoding.Default))
            {
                var section = new T();
                using (var reader = XmlReader.Create(stream, new XmlReaderSettings { CloseInput = true }))
                {
                    section.GetType().
                        GetMethod("DeserializeSection", BindingFlags.NonPublic | BindingFlags.Instance).
                        Invoke(section, new object[] { reader });
                }
                return section;
            }
        }
        
    }
}