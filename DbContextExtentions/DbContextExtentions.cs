using System;
using System.Collections.Generic;
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
        public static void Update<TEntry>(this DbContext ctx, TEntry entity, params Expression<Func<TEntry, object>>[] dirtyFields) where TEntry : class
        {
            ctx.Set(typeof(TEntry)).Attach(entity);

            if (dirtyFields != null && dirtyFields.Length > 0)
            {
                dirtyFields.ToList().ForEach(field =>
                {
                    ctx.Entry(entity).Property(field).IsModified = true;
                });
            }
            else
            {
                ctx.Entry(entity).State = System.Data.EntityState.Modified;
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
    }
}
