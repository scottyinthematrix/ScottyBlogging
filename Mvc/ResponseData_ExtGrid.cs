using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc
{
    public class ResponseData_ExtGrid
    {
        public Array Data { get; set; }
        public Array Errors { get; set; }
        public int Total { get; set; }
    }
}