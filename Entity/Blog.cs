using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
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

        public ICollection<Entry> Entries { get; set; }
    }
}
