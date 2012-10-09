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

        private static void InitEnterpriseLibraryContainer(IUnityContainer container, string filePath)
        {
            // as a reference for EnterpriseLibraryContainer
            var configurator = new UnityContainerConfigurator(container);
            var configurationSource = new FileConfigurationSource(filePath);
            EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
            var locator = new UnityServiceLocator(container);
            EnterpriseLibraryContainer.Current = locator;
        }

        /// <summary>
        /// resolve and mapping from an existing object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="existingObject"></param>
        /// <param name="name"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public static T Resolve<T>(this IUnityContainer container, T existingObject, string name, params ResolverOverride[] overrides)
            where T : class
        {
            var interceptedObject = container.Resolve<T>(name, overrides);

            if (existingObject != null)
            {
                AutoMapper.Mapper.Map(existingObject, interceptedObject);
            }

            return interceptedObject;
        }
        /// <summary>
        /// resolve and mapping from an existing object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <param name="existingObject"></param>
        /// <param name="overrides"></param>
        /// <returns></returns>
        public static T Resolve<T>(this IUnityContainer container, T existingObject, params ResolverOverride[] overrides)
        {
            var interceptedObject = container.Resolve<T>(overrides);

            if (existingObject != null)
            {
                AutoMapper.Mapper.Map(existingObject, interceptedObject);
            }

            return interceptedObject;
        }
    }
}
