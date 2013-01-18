using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc
{
    public static class ControllerExtensions
    {
        public static string GetBlogName(this Controller controller)
        {
            return controller.RouteData.Values["blog"].ToString();
        }
    }
}