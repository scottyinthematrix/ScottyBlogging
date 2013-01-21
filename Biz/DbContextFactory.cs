using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using ScottyApps.Utilities.EntlibExtensions;

namespace ScottyApps.ScottyBlogging.Biz
{
    public class DbContextFactory
    {
        public static T CreateDbContext<T>()
            where T : DbContext
        {
            return EntlibUtils.Container.Resolve<T>();
        }
    }
}
