using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.Unity;
using ScottyApps.ScottyBlogging.Entity;
using ScottyApps.Utilities.EntlibExtensions;
using Wintellect.PowerCollections;
using ScottyApps.Utilities.DbContextExtensions;

namespace ScottyApps.ScottyBlogging.Biz
{
    public class BloggingBiz
    {
        public virtual T FindByKey<T>(params object[] keyValues)
            where T : class
        {
            using (var ctx = DbContextFactory.CreateDbContext<BloggingContext>())
            {
                return ctx.FindByKey<T>(keyValues);
            }
        }

        public virtual List<T> FindAll<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            using (var ctx = DbContextFactory.CreateDbContext<BloggingContext>())
            {
                return ctx.FindAll<T>(predicate).ToList();
            }
        }

        public virtual T FindSingle<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            using (var ctx = DbContextFactory.CreateDbContext<BloggingContext>())
            {
                return ctx.FindSingle<T>(predicate);
            }
        }

        public virtual List<Blog> FindBlogs(/* TODO: accept expression as a predicate builder */)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public virtual Blog GetBlog(string blogName)
        {
            using (var ctx = EntlibUtils.Container.Resolve<BloggingContext>())
            {
                return ctx.Blogs/*.Include("Writer")*/.SingleOrDefault(b => b.Name.ToLower() == blogName.ToLower());
            }
        }

        public virtual List<Blog> GetBlogsForWriter(Writer writer)
        {
            var container = EntlibUtils.Container;

            using (var ctx = container.Resolve<BloggingContext>())
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
        public virtual List<Article> GetArticlesForBlog(string blogName)
        {
            using (var ctx = DbContextFactory.CreateDbContext<BloggingContext>())
            {
                var query = from a in ctx.Articles
                            where a.Blog.Name.ToLower() == blogName.ToLower()
                            select a;
                return query.ToList();
            }
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

        public virtual void SaveArticle(Article article, bool isUpdate)
        {
            if (isUpdate)
            {
                article.MarkAsModified<Article>(a => new { a.Title, a.LegalTitle });
            }
            else
            {
                article.MarkAsAdded();
            }
            var container = EntlibUtils.Container;
            using (var ctx = container.Resolve<BloggingContext>())
            {
                ctx.SaveChanges(article);
            }
        }
    }
}
