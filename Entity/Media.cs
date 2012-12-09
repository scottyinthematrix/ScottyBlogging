using System.ComponentModel.DataAnnotations;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Media
    {
        public string ID { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }

        public virtual MediaGroup MediaGroup { get; set; }
    }
}
