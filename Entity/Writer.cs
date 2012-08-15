using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Writer
    {
        [Key]
        public string ID { get; set; }
        [MaxLength(50)]
        public string Alias { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        public ICollection<Blog> Blogs { get; set; }

        public ICollection<Entry> Entries { get; set; }
    }
}
