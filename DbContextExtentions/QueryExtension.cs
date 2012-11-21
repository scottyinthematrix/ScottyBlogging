using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScottyApps.Utilities.DbContextExtentions
{
    public class OrderByExp<T, TKey>
    {
        public Expression<Func<T, TKey>> Expression { get; set; }
        public bool IsDecending { get; set; }
    }
    public static class QueryExtension
    {
        public static List<T> ToPagedList<T>(this IOrderedQueryable<T> query, int pageIndex, int pageSize, out int totalRecords)
        {
            // TODO
            // the idea here is:
            // first, we use query.ToTraceString() to get the orginal query sql
            // then, we pass the sql to a stored procedure which should already exists prior to the query starts
            // (the procedure will be responsible for all the rest)

            // this will definitely requires calling ExcecuteSQLCommand or something
            //var origSql = (query as ObjectQuery<T>).ToTraceString();
            //query.ToList();

            // how to assemble the result from database back into our Entities?
            totalRecords = 0;
            var entities = new List<T>();

            var pageQuery = from e in query.Skip(pageIndex * pageSize).Take(pageSize)
                            let count = query.Count()
                            select new
                            {
                                Count = count,
                                Entity = e
                            };

            var data = pageQuery.ToList();
            if (data.Count > 0)
            {
                totalRecords = data[0].Count;
                data.ForEach(e => entities.Add(e.Entity));
            }

            return entities;
        }
        public static List<T> ToPagedList<T, TKey>(this IQueryable<T> query, int pageIndex, int pageSize, out int totalRecords, params OrderByExp<T, dynamic>[] sortExps)
        {
            IOrderedQueryable<T> orderedQuery = query.MultipleOrderBy(sortExps);
            return ToPagedList(orderedQuery, pageIndex, pageSize, out totalRecords);
        }

        private static IOrderedQueryable<T> MultipleOrderBy<T>(this IQueryable<T> query, params OrderByExp<T, dynamic>[] sortExps)
        {
            IOrderedQueryable<T> orderedQuery = null;
            if (sortExps == null || sortExps.Length < 1)
            {
                throw new InvalidOperationException("at least one sort expression is required.");
            }

            bool firstOne = true;
            foreach (var exp in sortExps)
            {
                if (firstOne)
                {
                    orderedQuery = exp.IsDecending
                                        ? query.OrderByDescending(exp.Expression)
                                        : query.OrderBy(exp.Expression);
                    firstOne = false;
                }
                else
                {
                    orderedQuery = exp.IsDecending
                                       ? orderedQuery.ThenByDescending<T, dynamic>(exp.Expression)
                                       : orderedQuery.ThenBy<T, dynamic>(exp.Expression);
                }
            }

            return orderedQuery;
        }
    }
}
