using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration;
using Microsoft.Practices.Unity.Utility;

namespace ScottyApps.Utilities.EntlibExtensions
{
    public static class UnityExtensions
    {
        /// <summary>
        /// this will load the configuration from separate external file,
        /// and set EnterpriseLibrary.Current at the same time to current used container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="filePath">external full file path</param>
        /// <param name="unitySectionName">unity section name (default to "unity")</param>
        /// <param name="containerName">unity container name (defaults to emtpy)</param>
        /// <returns></returns>
        public static void LoadConfiguration(this IUnityContainer container,
            string filePath, string unitySectionName = "unity", string containerName = "")
        {
            Guard.ArgumentNotNullOrEmpty(filePath, "filePath");

            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = filePath };
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var section = (UnityConfigurationSection)config.GetSection(unitySectionName);

            InitEnterpriseLibraryContainer(container, filePath);
            container.LoadConfiguration(section, containerName);
        }

        public static void InitEnterpriseLibraryContainer(IUnityContainer container, string filePath)
        {
            // as a reference for EnterpriseLibraryContainer
            var configurator = new UnityContainerConfigurator(container);
            var configurationSource = new FileConfigurationSource(filePath);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
            var locator = new UnityServiceLocator(container);
            EnterpriseLibraryContainer.Current = locator;
        }

    }
}
