using System.ComponentModel.DataAnnotations;
using ScottyApps.Utilities.DbContextExtensions;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Media : EntityBase
    {
        public string ID { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }

        public virtual MediaGroup MediaGroup { get; set; }
    }
}
