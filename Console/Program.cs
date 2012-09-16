using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottyApps.ScottyBlogging.Entity;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new BloggingContext())
            {
                var query = ctx.Blogs.ToList();
                if (query.Count == 0)
                {
                    System.Console.WriteLine("No Blogs found.");
                }
                else
                {
                    System.Console.WriteLine("{0} blogs found.", query.Count);
                }
            }
        }
    }
}
