﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ScottyApps.ScottyBlogging.Entity
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Blog : EntityBase
    {
        [Required]
        public string ID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }

        public virtual Writer Writer { get; set; }

        public virtual ICollection<Entry> Entries { get; set; }

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
