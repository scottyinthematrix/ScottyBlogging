using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using ScottyApps.ScottyBlogging.Resx;
using ScottyApps.Utilities.EntlibExtensions;
using ScottyApps.Utilities.DbContextExtentions;

namespace ScottyApps.ScottyBlogging.Entity
{
    public static class EntityBaseExtension
    {
        public static void UpdateToStore<TEntity>(this TEntity entity,
                                                  params Expression<Func<TEntity, object>>[] dirtyFields)
            where TEntity : EntityBase
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            using (var context = EntlibUtils.Container.Resolve<BloggingContext>())
            {
                context.Update(entity, dirtyFields);
            }
        }
    }
    public class EntityBase
    {
        public virtual void AddToStore()
        {
            using (var context = EntlibUtils.Container.Resolve<BloggingContext>())
            {
                context.Set(this.GetType()).Add(this);
                context.SaveChanges();
            }
        }

        public virtual void DeleteFromStore()
        {
            using (var context = EntlibUtils.Container.Resolve<BloggingContext>())
            {
                context.Set(this.GetType()).Attach(this);
                context.Entry(this).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }

        public override string ToString()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            return JsonConvert.SerializeObject(this, Formatting.Indented, settings);
        }
    }
}
