using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScottyApps.Utilities.DbContextExtentions
{
    public static class DbContextExtentions
    {
        public static void Update<TEntry>(this DbContext ctx, TEntry entity, params Expression<Func<TEntry, object>>[] dirtyFields) where TEntry : class
        {
            ctx.Set(typeof(TEntry)).Attach(entity);

            if (dirtyFields != null && dirtyFields.Length > 0)
            {
                dirtyFields.ToList().ForEach(field => {
                    ctx.Entry(entity).Property(field).IsModified = true;
                });
            }
            else
            {
                ctx.Entry(entity).State = System.Data.EntityState.Modified;
            }

            ctx.SaveChanges();
        }
    }
}
