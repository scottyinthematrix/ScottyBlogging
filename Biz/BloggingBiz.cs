using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottyApps.ScottyBlogging.Entity;

namespace ScottyApps.ScottyBlogging.Biz
{
    public class BloggingBiz
    {
        #region Finders
        public List<Blog> FindBlogs(/* TODO: accept expression as a predicate builder */)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        #endregion


        public List<Blog> GetBlogsForWriter(Writer writer)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public List<Article> GetArticlesForBlog(Blog blog)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public List<Gossip> GetGossipsForBlog(Blog blog)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public List<Article> GetArticlesForTag(Tag tag)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }

        public List<Gossip> GetGossipsForTag(Tag tag)
        {
            throw new NotImplementedException("hurry up, scotty!");
        }
    }
}
