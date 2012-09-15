using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.Utility;

namespace ScottyApps.Utilities.EntlibExtensions
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer LoadConfiguration2(this IUnityContainer container,
    string filePath)
        {
            return container.LoadConfiguration2(filePath, "unity", "");
        }
        public static IUnityContainer LoadConfiguration2(this IUnityContainer container,
            string filePath, string unitySectionName = "unity", string containerName = "")
        {
            Guard.ArgumentNotNullOrEmpty(filePath, "filePath");

            var config = ConfigurationManager.OpenExeConfiguration(filePath);
            var section = (UnityConfigurationSection)config.GetSection(unitySectionName);
            return container.LoadConfiguration(section, containerName);
        }
    }
}
