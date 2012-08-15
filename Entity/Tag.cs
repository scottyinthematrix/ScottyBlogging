using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }

        public Tag ParentTag { get; set; }

        public ICollection<Tag> ChildTags { get; set; }
    }
}
