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
        public static IUnityContainer LoadConfiguration(this IUnityContainer container,
            string filePath, string unitySectionName = "unity", string containerName = "")
        {
            Guard.ArgumentNotNullOrEmpty(filePath, "filePath");

            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var section = (UnityConfigurationSection)config.GetSection(unitySectionName);

            return container.LoadConfiguration(section, containerName);
        }
    }
}
