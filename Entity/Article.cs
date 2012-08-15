using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Article : Entry
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
