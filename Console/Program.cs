using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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
using ScottyApps.Utilities.DbContextExtentions;
using Wintellect.PowerCollections;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapper.Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Blog, Blog>();
                });
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

            //var query = ctx.Blogs.ToList();
            //if (query.Count == 0)
            //{
            //    System.Console.WriteLine("No Blogs found.");
            //}
            //else
            //{
            //    System.Console.WriteLine("{0} blogs found.", query.Count);
            //}

            //using(var ctx = container.Resolve<BloggingContext>())
            //{
            //    int rows = ctx.Blogs.Delete(b => b.ID == "hello");
            //}

            #region Test for Paging

            //using (var ctx = container.Resolve<BloggingContext>())
            //{
            //    var query = from t in ctx.Tags
            //                where t.ParentTag.Name == "Web"
            //                orderby t.Name, t.Description descending
            //                select t;
            //    var query2 = from t in ctx.Tags
            //                 where t.ParentTag.Name == "Web"
            //                 select t;
            //    int count;
            //    //var result = query.ToPagedList(2, 1, out count);
            //    var result = query2.ToPagedList<Tag>(
            //        2, 1, out count,
            //        new Pair<Expression<Func<Tag, dynamic>>, bool>(t => t.Name, false),
            //        new Pair<Expression<Func<Tag, dynamic>>, bool>(t => t.Description, true));
            //}

            #endregion


            var biz = container.Resolve<BloggingBiz>();
            var blogs = biz.GetBlogsForWriter(new Writer { Email = "scotty.cn@gmail.com" });

            if (blogs == null || blogs.Count == 0)
            {
                System.Console.WriteLine("no blogs found.");
                return;
            }

            var blog = blogs[0];
            blog.Url = "http://www.baidu.com";
            blog.Writer.Alias = "scotty";

            using (var ctx = container.Resolve<BloggingContext>())
            {
                var triples = new List<Triple<object, EntityState, string[]>>
                {
                    new Triple<object, EntityState, string[]>(blog, EntityState.Modified, new[]{"Url"}),
                    new Triple<object, EntityState, string[]>(blog.Writer, EntityState.Modified, new[]{"Alias"})
                };
                ctx.SaveChanges(triples);
            }

            //var interceptedObj = EntlibUtils.Container.Resolve<Blog>(blog);
            //interceptedObj.Url = "http://www.g.cn";
            //interceptedObj.UpdateToStore<BloggingContext>(b => b.Url);
            return;
        }
    }
}
