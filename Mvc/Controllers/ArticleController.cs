using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScottyApps.ScottyBlogging.Entity;

namespace Mvc.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Article/

        public ActionResult Index()
        {
            var articles = new List<Article>
                               {
                                   new Article{Body = "test", CreateDate = DateTime.Now, ID=Guid.NewGuid().ToString(),Title = "test title"}
                               };
            //ViewBag.Articles = articles;
            return View(articles);
        }

    }
}
