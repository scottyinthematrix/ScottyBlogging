using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottyApps.Utilities.DbContextExtentions
{
    public static class QueryExtension
    {
        public static List<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex, int pageSize, out int totalRecords)
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
            return null;
        }
    }
}
