using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Container.Config;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Container
{
    public class ContainerManager
    {
        private const string ConfigFolderName = @"Config";
        private const string TenantConfigName = @"Tenants.config";
        private static ContainerManager instance;

        private ContainerManager()
        {
            var assemblyRootPath = Assembly.GetExecutingAssembly().GetAssemblyRootPath();
            var confFullPath = Path.Combine(assemblyRootPath, ConfigFolderName, TenantConfigName);
            var configSection = new ConfigurationSectionReader<TenantsConfigurationSection>().GetSection(confFullPath);
            var tenants = new List<TenantConfiguration>();
            foreach (var tenant in configSection.Tenants.OfType<TenantConfigurationElement>())
            {
                var tenantConf = new TenantConfiguration
                                     {
                                         TenantName = tenant.Id
                                     };
                var tenantUnityConfigPath = Path.Combine(assemblyRootPath, ConfigFolderName, tenant.UnityConfigPath);
                var tenantUnityConfigSection = new ConfigurationSectionReader<UnityConfigurationSection>().GetSection(tenantUnityConfigPath);
                var unityContainer = new UnityContainer();
                unityContainer.LoadConfiguration(tenantUnityConfigSection);
                unityContainer.RegisterInstance(new ConnectionManager
                                                    {
//                                                        ConnectionString = tenant.ConnectionString
                                                    },
                                                new ContainerControlledLifetimeManager());
                tenantConf.UnityContainer = unityContainer;
                tenants.Add(tenantConf);
            }
            Tenants = tenants.ToArray();
        }

        public static ContainerManager Instance
        {
            get { return instance ?? (instance = new ContainerManager()); }
        }

        public TenantConfiguration[] Tenants { get; private set; }
    }

    public class TenantConfiguration
    {
        public string TenantName { get; set; }
        public IUnityContainer UnityContainer { get; set; }
    }

    public class ConnectionManager
    {
        public string ConnectionString { get; set; }
    }

    public static class AssemblyExtentions
    {
        public static string GetAssemblyRootPath(this Assembly assembly)
        {
            return Path.GetDirectoryName(new Uri(assembly.CodeBase).LocalPath);
        }
    }
}