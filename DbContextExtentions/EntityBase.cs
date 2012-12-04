using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using ScottyApps.Utilities.EntlibExtensions;

namespace ScottyApps.Utilities.DbContextExtentions
{
    public static class EntityBaseExtension
    {
        //public static void UpdateToStore<TEntity, TContext>(this TEntity entity,
        //                                          params Expression<Func<TEntity, object>>[] dirtyFields)
        //    where TEntity : EntityBase
        //    where TContext : DbContext
        //{
        //    if (entity == null)
        //    {
        //        throw new ArgumentNullException("entity");
        //    }

        //    using (var ctx = EntlibUtils.Container.Resolve<TContext>())
        //    {
        //        ctx.Update(entity, dirtyFields);
        //    }
        //}
    }

    [Serializable]
    public class EntityBase
    {
        private readonly Guid _entityGuid = Guid.NewGuid();
        internal Guid EntityGuid
        {
            get { return _entityGuid; }
        }
        private EntityState _state = EntityState.Unchanged;
        public EntityState State
        {
            get { return _state; }
            set { _state = value; }
        }
        public Expression<Func<EntityBase, object>>[] DirtyFields { get; set; }

        public virtual void AddToStore<TContext>()
            where TContext : DbContext
        {
            using (var ctx = EntlibUtils.Container.Resolve<TContext>())
            {
                ctx.Set(GetType()).Add(this);
                ctx.SaveChanges();
            }
        }
        public virtual void DeleteFromStore<TContext>()
            where TContext : DbContext
        {
            using (var ctx = EntlibUtils.Container.Resolve<TContext>())
            {
                ctx.Set(this.GetType()).Attach(this);
                ctx.Entry(this).State = EntityState.Deleted;

                ctx.SaveChanges();
            }
        }
        public virtual void UpdateToStore<TContext>(params Expression<Func<EntityBase, object>>[] dirtyFields)
            where TContext : DbContext
        {
            using (var ctx = EntlibUtils.Container.Resolve<TContext>())
            {
                ctx.Update(this, dirtyFields);
            }
        }

    }
}
