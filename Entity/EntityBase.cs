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

namespace ScottyApps.ScottyBlogging.Entity
{
    [Serializable]
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

        public virtual void UpdateToStore()
        {
            using (var context = EntlibUtils.Container.Resolve<BloggingContext>())
            {
                context.Set(this.GetType()).Attach(this);
                context.Entry(this).State = EntityState.Modified;

                context.SaveChanges();
            }
        }
        // TODO should be improved for elegancy
        protected virtual void UpdateToStore<TEntity>(params Expression<Func<TEntity, object>>[] dirtyFields)
            where TEntity : EntityBase
        {
            var entity = this as TEntity;
            if (entity == null)
            {
                throw new InvalidOperationException(ScottyBloggingResx.exMsg_ObjectCanUpdateOnlyItself);
            }

            using (var context = EntlibUtils.Container.Resolve<BloggingContext>())
            {
                context.Set(this.GetType()).Attach(this);
                var entry = context.Entry(entity);

                if (dirtyFields != null && dirtyFields.Length > 0)
                {
                    dirtyFields.ToList().ForEach(expr =>
                                                     {
                                                         var propName = ((MemberExpression)expr.Body).Member.Name;
                                                         entry.Property(propName).IsModified = true;
                                                     });
                }
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

        //public override string ToString()
        //{
        //    JsonSerializerSettings settings = new JsonSerializerSettings();
        //    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        //    return JsonConvert.SerializeObject(this, Formatting.Indented, settings);
        //}
    }
}
