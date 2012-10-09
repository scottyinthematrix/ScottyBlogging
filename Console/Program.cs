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
using Newtonsoft.Json;
using ScottyApps.ScottyBlogging.Biz;
using ScottyApps.ScottyBlogging.Entity;
using ScottyApps.Utilities.EntlibExtensions;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var obj = new { FirstName = "scotty", LastName = "yang" };
            //var obj2 = new Blog { ID = Guid.NewGuid().ToString(), Name = "scottyinthematrix", Url = "www.g.cn" };
            //var s = JsonConvert.SerializeObject(obj2);
            //System.Console.WriteLine(s);
            //var s2 = JsonConvert.ToString(obj2);
            //System.Console.WriteLine(s2);

            //return;

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

                if (blogs == null || blogs.Count == 0)
                {
                    System.Console.WriteLine("no blogs found.");
                    return;
                }

                var blog = blogs[0];
                System.Console.WriteLine(blog.ToString());

                var interceptedObj = EntlibUtils.Container.Resolve<Blog>(blog);
                interceptedObj.Url = "http://www.g.cn";
                interceptedObj.UpdateToStore();
            }
        }
    }
}
