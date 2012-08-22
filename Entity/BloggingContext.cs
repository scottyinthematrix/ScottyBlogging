using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ScottyApps.ScottyBlogging.Entity
{
    public class BloggingContext : DbContext
    {
        public BloggingContext()
            : base("Blogging")
        {
            Database.SetInitializer<BloggingContext>(new DropCreateDatabaseIfModelChanges<BloggingContext>());
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Gossip> Gossips { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Writer> Writers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // many-to-many mapping between Entries and Tags
            modelBuilder.Entity<Entry>().
                HasMany(a => a.Tags)
                .WithMany(t => t.Entries).Map(m =>
                {
                    m.MapLeftKey("EntryID");
                    m.MapRightKey("TagID");
                    m.ToTable("EntryTags");
                });
        }
    }
}
