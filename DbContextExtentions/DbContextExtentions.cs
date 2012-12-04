using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScottyApps.Utilities.DbContextExtentions
{
    public static class DbContextExtentions
    {
        public static void Update<TEntry>(this DbContext ctx, TEntry entity, params Expression<Func<TEntry, object>>[] dirtyFields)
            where TEntry : EntityBase
        {
            ctx.Set(typeof(TEntry)).Attach(entity);

            // TODO
            // test below code works as expected when passing non-null dirty fields
            ctx.Entry(entity).State = System.Data.EntityState.Modified;
            if (dirtyFields != null && dirtyFields.Length > 0)
            {
                dirtyFields.ToList().ForEach(field =>
                {
                    ctx.Entry(entity).Property(field).IsModified = true;
                });
            }

            ctx.SaveChanges();
        }

        public static int Delete<TEntry>(this DbSet<TEntry> dbSet, Expression<Func<TEntry, bool>> predicate)
            where TEntry : class
        {
            var query = dbSet.Where(predicate);

            string sql;
            object[] parameters;
            BuildDeleteSql<TEntry>(GetObjectQueryFromDbQuery(query as DbQuery<TEntry>), out sql, out parameters);

            DbContext ctx = GetDbContextFromDbSet(dbSet);
            if (ctx == null)
            {
                throw new Exception("failed on getting DbContext from DbSet");
            }

            int rowsAffected = ctx.Database.ExecuteSqlCommand(sql, parameters);
            return rowsAffected;
        }
        private static void BuildDeleteSql<TEntry>(ObjectQuery query, out string sql, out object[] parameters)
            where TEntry : class
        {
            sql = string.Empty;
            parameters = null;

            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            string origSql = query.ToTraceString().Replace(Environment.NewLine, " ");
            int idxFrom = origSql.IndexOf("from", StringComparison.OrdinalIgnoreCase);
            int idxWhere = origSql.IndexOf("where", StringComparison.OrdinalIgnoreCase);
            string tableWithAlias = origSql.Substring(idxFrom + 4, idxWhere - (idxFrom + 4));
            int idxAs = tableWithAlias.IndexOf("as", StringComparison.OrdinalIgnoreCase);
            string alias = tableWithAlias.Substring(idxAs + 2);

            sql = string.Format("delete {0} from {1} {2}", alias, tableWithAlias, origSql.Substring(idxWhere));
            parameters = query.Parameters.ToArray();
        }
        private static DbContext GetDbContextFromDbSet<TEntry>(DbSet<TEntry> dbSet)
            where TEntry : class
        {
            var binding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            object internalSet = dbSet.GetType().GetField("_internalSet", binding).GetValue(dbSet);
            object internalContext = internalSet.GetType().GetProperty("InternalContext", binding).GetValue(internalSet, null);
            object context = internalContext.GetType().GetProperty("Owner", binding).GetValue(internalContext, null);

            return context as DbContext;
        }
        private static ObjectQuery GetObjectQueryFromDbQuery<TEntry>(DbQuery<TEntry> query)
        {
            var binding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var internalQuery = query.GetType().GetProperty("InternalQuery", binding).GetValue(query);
            var objectQuery = internalQuery.GetType().GetProperty("ObjectQuery", binding).GetValue(internalQuery);
            return objectQuery as ObjectQuery;
        }

        public static void SaveChanges<TContext, TEntry>(this TContext ctx, List<TEntry> toBeUpdatedEntities)
            where TContext : DbContext
            where TEntry : EntityBase
        {
            if (toBeUpdatedEntities == null || toBeUpdatedEntities.Count == 0)
            {
                return;
            }

            Dictionary<Guid, EntityBase> objDic = new Dictionary<Guid, EntityBase>();

            using (ctx)
            {
                foreach (var entry in toBeUpdatedEntities)
                {
                    AttachObjectWithState(ctx, entry, ref objDic);
                }

                ctx.SaveChanges();
            }
        }
        private static void AttachObjectWithState<TContext, TEntry>(TContext ctx, TEntry entry, ref Dictionary<Guid, EntityBase> objDic)
            where TContext : DbContext
            where TEntry : EntityBase
        {
            if (!objDic.ContainsKey(entry.EntityGuid))
            {
                // attach object itself
                switch (entry.State)
                {
                    case EntityState.Added:
                        ctx.Set(typeof(TEntry)).Add(entry);
                        break;
                    case EntityState.Deleted:
                        ctx.Set(typeof(TEntry)).Remove(entry);
                        break;
                    case EntityState.Modified:
                        ctx.Set(typeof(TEntry)).Attach(entry);
                        ctx.Entry(entry).State = EntityState.Modified;
                        if (entry.DirtyFields != null && entry.DirtyFields.Length > 0)
                        {
                            var dbEntry = ctx.Entry((EntityBase)entry);
                            entry.DirtyFields.ToList().ForEach(f => dbEntry.Property(f).IsModified = true);
                        }
                        break;
                }
            }
            // attach navigation objects belong to this object
            List<PropertyInfo> navProps = GetNavProps(entry);
            if (navProps != null && navProps.Count > 0)
            {
                foreach (var prop in navProps)
                {
                    var val = prop.GetValue(entry, null);
                    if (val == null) continue;

                    if (typeof(EntityBase).IsAssignableFrom(prop.PropertyType))    // individual child
                    {
                        var entity = val as EntityBase;
                        AttachObjectWithState(ctx, entity, ref objDic);
                    }
                    else
                    {
                        var collection = val as IEnumerable;
                        if (collection != null)
                        {
                            foreach (var entity in collection.Cast<object>().Where(entity => entity != null))
                            {
                                AttachObjectWithState(ctx, entity as EntityBase, ref objDic);
                            }
                        }
                    }
                }
            }

        }
        private static List<PropertyInfo> GetNavProps<TEntry>(TEntry entry)
            where TEntry : EntityBase
        {
            List<PropertyInfo> propList = new List<PropertyInfo>();
            Type type = typeof(EntityBase);

            foreach (var p in entry.GetType().GetProperties())
            {
                if (p.PropertyType.IsGenericType
                    && p.PropertyType.Name == "ICollection`1")
                {
                    Type[] argTypes = p.PropertyType.GetGenericArguments();
                    if (argTypes.Length == 1
                        && type.IsAssignableFrom(argTypes[0]))
                    {
                        propList.Add(p);
                    }
                }
                else if (type.IsAssignableFrom(p.PropertyType))
                {
                    propList.Add(p);
                }
            }

            return propList;
        }
        public static TEntry MarkAsAdded<TEntry>(this TEntry entry)
            where TEntry : EntityBase
        {
            entry.State = EntityState.Added;
            return entry;
        }
        public static TEntry MarkAsDeleted<TEntry>(this TEntry entry)
            where TEntry : EntityBase
        {
            entry.State = EntityState.Deleted;
            return entry;
        }
        public static TEntry MarkAsModified<TEntry>(this TEntry entry)
            where TEntry : EntityBase
        {
            entry.State = EntityState.Modified;
            return entry;
        }

    }
}
