using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Mvc
{
    public static class ConfigHelper
    {
        private const string Key_EntlibConfigPath = "EntlibConfigPath";
        private const string Key_PostsServerPaths = "PostsServerPaths";
        public static string GetEntlibConfigPath()
        {
            return WebConfigurationManager.AppSettings[Key_EntlibConfigPath];
        }
        public static string GetRandomPostsServerPath()
        {
            var pathStr = WebConfigurationManager.AppSettings[Key_PostsServerPaths];
            if (string.IsNullOrEmpty(pathStr))
            {
                throw new ArgumentNullException(Key_PostsServerPaths);
            }

            var pathArr = pathStr.Split(';');
            if (pathArr.Length < 2)
            {
                return pathArr[0];
            }

            Random rnd = new Random();
            // NOTE A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. 
            return pathArr[rnd.Next(0, pathArr.Length)];
        }
    }
}