﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ScottyApps.Utilities.DbContextExtensions;

namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
    public class Tag : EntityBase
    {
        [Key]
        public string Name { get; set; }

        public Tag ParentTag { get; set; }

        public ICollection<Tag> ChildTags { get; set; }

        public ICollection<Entry> Entries { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

    }
}
