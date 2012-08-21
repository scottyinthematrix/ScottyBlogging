using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
    public class Tag
    {
        [Key]
        public string Name { get; set; }

        public Tag ParentTag { get; set; }

        public ICollection<Tag> ChildTags { get; set; }

        public ICollection<Article> Articles
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
