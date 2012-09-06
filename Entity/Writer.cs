using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using ScottyApps.ScottyBlogging.Resx;
using ScottyApps.Utilities.DbContextExtentions;

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
            using (BloggingContext ctx = new BloggingContext())
            {
                var existingWriter = ctx.Writers.Single(w => w.Email == this.Email);
                if (existingWriter != null)
                {
                    return RegisterStatus.UserExisting;
                }

                if (string.IsNullOrEmpty(this.ID))
                {
                    this.ID = Guid.NewGuid().ToString();
                }

                // TODO encrypt the password
                ctx.Writers.Add(this);
                ctx.SaveChanges();
                return RegisterStatus.OK;
            }
        }
        public UserValidationStatus Validate()
        {
            using (BloggingContext ctx = new BloggingContext())
            {
                var existingWriter = ctx.Writers.Single(w => w.Email == this.Email);
                if (existingWriter == null)
                {
                    return UserValidationStatus.UserNotExist;
                }

                // TODO should decrypt the password then do the comparison
                if (existingWriter.Password == this.Password)
                {
                    return UserValidationStatus.OK;
                }
                else
                {
                    return UserValidationStatus.WrongPassword;
                }
            }
        }
        public void Update()
        {
            if (string.IsNullOrEmpty(this.ID))
            {
                throw new InvalidOperationException(ScottyBloggingResx.exMsg_EmptyWriterID);
            }

            using (BloggingContext ctx = new BloggingContext())
            {
                //ctx.Writers.Attach(this);
                //ctx.Entry<Writer>(this).Property(w => w.Alias).IsModified = true;
                //ctx.SaveChanges();
                ctx.Update(this, w => w.Alias);
            }
        }
        public void Delete()
        {
            if (string.IsNullOrEmpty(this.ID))
            {
                throw new InvalidOperationException(ScottyBloggingResx.exMsg_EmptyWriterID);
            }

            using (BloggingContext ctx = new BloggingContext())
            {
                ctx.Writers.Remove(this);
                ctx.SaveChanges();
            }
        }
    }
}
