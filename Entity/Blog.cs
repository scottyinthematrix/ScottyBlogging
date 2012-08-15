using System.ComponentModel.DataAnnotations;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Blog
    {
        [Required]
        public string ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }

        public Writer Writer { get; set; }
    }
}
