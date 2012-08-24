using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
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

        public Blog Blog { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public void Add()
        {
        }
        public void Update()
        {
        }
        public void Delete()
        {
        }
    }
}
