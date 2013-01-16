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

        public ActionResult Get()
        {
            return Json(new { Name = "scotty", Epoch = new DateTime(1970, 1, 1), BirthDate = new DateTime(1981, 7, 10) }, JsonRequestBehavior.AllowGet);

            // NOTE - will generate below json string:
            // {"Name":"scotty","BirthDate":"\/Date(363542400000)\/"}
        }
    }
}
