using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Entry
    {
        [Key]
        public string ID { get; set; }
        [Required]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime ModifyDate { get; set; }
        [MaxLength]
        public string Body { get; set; }

        public Writer Writer { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public string PermUrl { get; set; }
    }
}
