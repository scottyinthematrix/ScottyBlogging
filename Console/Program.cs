using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity.InterceptionExtension.Configuration;
using ScottyApps.ScottyBlogging.Biz;
using ScottyApps.ScottyBlogging.Entity;
using ScottyApps.Utilities.EntlibExtensions;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = ConfigurationManager.AppSettings["EntlibConfigPath"];
            EntlibUtils.InitializeUnityContainer(filePath);
            //IUnityContainer container = new UnityContainer();
            //container.AddNewExtension<Interception>();
            //container.RegisterType<BloggingBiz>(
            //    new ContainerControlledLifetimeManager(),
            //    new Interceptor<VirtualMethodInterceptor>(),
            //    new InterceptionBehavior<PolicyInjectionBehavior>());

            //container.RegisterType<BloggingContext>(
            //    new InjectionConstructor("Blogging"));

            //container.Configure<Interception>()
            //    .AddPolicy("policy4Biz")
            //        .AddMatchingRule<AssemblyMatchingRule>(new InjectionConstructor("ScottyApps.ScottyBlogging.Biz"))
            //        .AddCallHandler<Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection.LogCallHandler>(
            //            new InjectionConstructor());

            //UnityExtensions.InitEnterpriseLibraryContainer(container, filePath);

            //EntlibUtils.Container = container;
            var container = EntlibUtils.Container;

            using (var ctx = container.Resolve<BloggingContext>())
            {
                //var query = ctx.Blogs.ToList();
                //if (query.Count == 0)
                //{
                //    System.Console.WriteLine("No Blogs found.");
                //}
                //else
                //{
                //    System.Console.WriteLine("{0} blogs found.", query.Count);
                //}

                var biz = container.Resolve<BloggingBiz>();
                var blogs = biz.GetBlogsForWriter(new Writer { Email = "scotty.cn@gmail.com" });
                System.Console.WriteLine(blogs.Count);
            }
        }
    }
}
