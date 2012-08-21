using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
    public class Article : Entry
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
