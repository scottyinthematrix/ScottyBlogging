using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using ScottyApps.Utilities.DbContextExtentions;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class Media : EntityBase
    {
        public string ID { get; set; }
        [MaxLength(500)]
        public string Path { get; set; }

        public virtual MediaGroup MediaGroup { get; set; }

        public override void AddToStore<TContext>()
        {
            this.ID = string.IsNullOrEmpty(this.ID) ? Guid.NewGuid().ToString() : this.ID;
            base.AddToStore<TContext>();
        }
    }
}
