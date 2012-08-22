using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class MediaGroup
    {
        public string ID { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(500)]
        public int Description { get; set; }
    }
}
