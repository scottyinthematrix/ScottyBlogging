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
            return View();
        }

        private ActionResult Get()
        {
            return Json(new { Name = "scotty", Epoch = new DateTime(1970, 1, 1), BirthDate = new DateTime(1981, 7, 10) }, JsonRequestBehavior.AllowGet);

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
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ArticleId = string.Empty;
                ViewBag.ArticleContent = string.Empty;
                return View();
            }

            ViewBag.ArticleId = id;
            ViewBag.ArticleTitle = string.Format("Title of {0}", id);
            ViewBag.ArticleContent = string.Format("Here goes some content of the article of which the id is {0}", id);
            return View();
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateInput(false)]
        // TODO replace "articleEditor" with a friendly name via a customized ModelBinder?
        public ActionResult SaveEdit(string id, string title, string articleEditor)
        {
            // save to a file, mark the path to db
            return Json(new { ID = id, Content = articleEditor });

            // TODO in the future, this can be saved by a VCS, such as git/mercurial

            // a normal process for this should be:
            // 1 - the id will be set as the file name, .post will be the extension
            // 2 - there should be a separate folder for each writter on each blog
            // 3 - save the post to the destination folder with the filtered title as file name
            // 4 - update db to record the file path
        }
    }
}
