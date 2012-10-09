using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace ScottyApps.Utilities.EntlibExtensions
{
    public static class EntlibUtils
    {
        /// <summary>
        /// initialize the unity container
        /// this is meant to create the container and store it to somewhere for caching
        /// </summary>
        /// <param name="filePath"></param>
        public static void InitializeUnityContainer(string filePath)
        {
            Container = new UnityContainer();

            Container.LoadConfiguration(filePath);
        }

        public static IUnityContainer Container { get; private set; }
    }
}
