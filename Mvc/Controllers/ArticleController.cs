using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using ScottyApps.ScottyBlogging.Biz;
using ScottyApps.ScottyBlogging.Entity;
using Mvc;
using ScottyApps.Utilities.EntlibExtensions;
using FileIO = System.IO.File;

namespace Mvc.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            string blogName = this.GetBlogName();

            var container = EntlibUtils.Container;
            var biz = container.Resolve<BloggingBiz>();
            var data = biz.GetArticlesForBlog(blogName);
            return Json(new ResponseData_ExtGrid { Data = data.ToArray(), Total = 1 });
            //return Json(new { Name = "scotty", Epoch = new DateTime(1970, 1, 1), BirthDate = new DateTime(1981, 7, 10) }, JsonRequestBehavior.AllowGet);

            // NOTE - will generate below json string:
            // {"Name":"scotty","BirthDate":"\/Date(363542400000)\/"}
        }

        public ActionResult Read(string id)
        {
            return View();
        }

        public ActionResult Read2(string title)
        {
            return null;
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            // empty id means creating a new article,
            // otherwise, it means editing an existing one
            var editingExistingArticle = !string.IsNullOrEmpty(id);
            if (!editingExistingArticle)
            {
                ViewBag.ArticleId = string.Empty;
                ViewBag.ArticleTitle = string.Empty;
                ViewBag.ArticleContent = "~ put your content here ~";
                return View();
            }

            var biz = EntlibUtils.Container.Resolve<BloggingBiz>();
            var article = biz.FindByKey<Article>(id);
            if (article != null)
            {
                ViewBag.ArticleId = id;
                ViewBag.ArticleTitle = article.Title;
                ViewBag.ArticleContent = FileIO.ReadAllText(GetFullPostPath(article.Body, id));
            }
            return View();
        }

        private string GetFullPostPath(string serverPath, string id)
        {
            var blogName = this.GetBlogName();
            return Path.Combine(serverPath, blogName, id + ".post");
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateInput(false)]
        // TODO replace "articleEditor" with a friendly name via a customized ModelBinder?
        public ActionResult SaveEdit(string articleId, string title, string articleEditor)
        {
            // TODO in the future, this can be saved by a VCS, such as git/mercurial

            // a normal process for this should be:
            // 1 - the id will be set as the file name, .post will be the extension
            // 2 - there should be a separate folder for each writter on each blog
            // 3 - save the post to the destination folder with the filtered title as file name
            // 4 - update db to record the file path


            // TODO if it's a new one, more properties should be populated, such as writer

            var editingExistingArticle = !string.IsNullOrEmpty(articleId);
            var biz = EntlibUtils.Container.Resolve<BloggingBiz>();

            Article article = editingExistingArticle ? biz.FindByKey<Article>(articleId) : new Article { CreateDate = DateTime.Now };

            article.Title = title;

            var serverPath = editingExistingArticle ? article.Body : ConfigHelper.GetRandomPostsServerPath();
            var dest = GetFullPostPath(serverPath, article.ID);
            var folder = Path.GetDirectoryName(dest);
            article.Body = serverPath;

            var blogName = this.GetBlogName();
            var blog = biz.FindSingle<Blog>(b => b.Name.ToLower() == blogName.ToLower());
            blog.MarkAsUnchanged();
            article.Blog = blog;

            // NOTE i need the db updating and file io to be in the same transaction - seems it actually does NOT work as expected
            using (TransactionScope ts = new TransactionScope())
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                FileIO.WriteAllText(dest, articleEditor);
                biz.SaveArticle(article, editingExistingArticle);

                ts.Complete();
            }

            return RedirectToAction("Index");
        }
    }
}
