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
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        public DateTime? ModifyDate { get; set; }
        [MaxLength(int.MaxValue)]
        public string Body { get; set; }

        public virtual Writer Writer { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public string PermUrl { get; set; }

        public virtual Blog Blog { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public Entry()
        {
            this.ID = Guid.NewGuid().ToString();
        }
    }
}
