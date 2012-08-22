using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Media
    {
        public string ID { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }

        public MediaGroup MediaGroup { get; set; }
    }
}
