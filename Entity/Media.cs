using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Media : EntityBase
    {
        public string ID { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }

        public virtual MediaGroup MediaGroup { get; set; }

        public override void AddToStore()
        {
            if(string.IsNullOrEmpty(this.ID))
            {
                this.ID = Guid.NewGuid().ToString();
            }

            base.AddToStore();
        }
    }
}
