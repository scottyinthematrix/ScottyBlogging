using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Practices.Unity;
using ScottyApps.ScottyBlogging.Entity;
using ScottyApps.Utilities.EntlibExtensions;
using Wintellect.PowerCollections;
using ScottyApps.Utilities.DbContextExtensions;

namespace ScottyApps.ScottyBlogging.Biz
{
    public class BloggingBiz
    {
        public virtual List<Blog> FindBlogs(/* TODO: accept expression as a predicate builder */)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public virtual Article GetArticle(string id)
        {
            var container = EntlibUtils.Container;

            using (var ctx = container.Resolve<BloggingContext>())
            {
                return ctx.Articles.SingleOrDefault(a => a.ID == id);
            }
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
            var container = EntlibUtils.Container;

            using (var ctx = container.Resolve<BloggingContext>())
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
