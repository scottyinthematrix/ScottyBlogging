using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using ScottyApps.ScottyBlogging.Entity;
using ScottyApps.Utilities.EntlibExtensions;

namespace ScottyApps.ScottyBlogging.Biz
{
    public class BloggingBiz
    {
        #region Finders
        public virtual List<Blog> FindBlogs(/* TODO: accept expression as a predicate builder */)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        #endregion


        public virtual List<Blog> GetBlogsForWriter(Writer writer)
        {
            var container = EntlibUtils.Container;

            using(var ctx = container.Resolve<BloggingContext>())
            {
                var query = from b in ctx.Blogs.Include("Writer").AsNoTracking()
                            where b.Writer.Email == writer.Email
                            select b;

                return query.ToList();
            }
        }

        public virtual List<Article> GetArticlesForBlog(Blog blog)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public virtual List<Gossip> GetGossipsForBlog(Blog blog)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public virtual List<Article> GetArticlesForTag(Tag tag)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public virtual List<Gossip> GetGossipsForTag(Tag tag)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }
    }
}
