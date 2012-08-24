using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ScottyApps.ScottyBlogging.Entity
{
    [DataContract(IsReference = true)]
    public class Writer
    {
        [Key]
        public string ID { get; set; }
        [MaxLength(50)]
        public string Alias { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Blog> Blogs { get; set; }

        public ICollection<Entry> Entries { get; set; }

        public RegisterStatus Register()
        {
            throw new NotImplementedException("hurry up, scotty!");
        }
        public UserValidationStatus Validate()
        {
            throw new NotImplementedException("hurry up, scotty!");
        }
        public void Update()
        {
        }
        public void Delete()
        {
        }
    }
}
