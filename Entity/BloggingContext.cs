using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Database.SetInitializer<BloggingContext>(new BloggingInitializer());
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Gossip> Gossips { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Writer> Writers { get; set; }
        public DbSet<MediaGroup> MediaGroups { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<Vedio> Vedios { get; set; }
        public DbSet<Graph> Graphs { get; set; }

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

    internal class BloggingInitializer : DropCreateDatabaseIfModelChanges<BloggingContext>
    {
        protected override void Seed(BloggingContext context)
        {
            //base.Seed(context);

            context.Writers.Add(new Writer
            {
                ID = Guid.NewGuid().ToString(),
                Alias = "scotty_matrix",
                Email = "scotty.cn@gmail.com",
                Password = "cppfans",
                Blogs = new Collection<Blog>
                {
                    new Blog
                    {
                        ID=Guid.NewGuid().ToString(),
                        Name="Scotty In The Matrix",
                        Url="http://www.scottyinthematrix.com",
                        Entries=new Collection<Entry>
                        {
                            new Article
                            {
                                ID = Guid.NewGuid().ToString(),
                                CreateDate = DateTime.Now,
                                Title = "welcome",
                                Body = "welcome to my blog",
                                Tags = new Collection<Tag>
                                {
                                    new Tag
                                    {
                                        Name = "TestTag",
                                        Description = "just for a test for right now"
                                    }
                                }
                            }
                        }
                    }
                }
            });
            context.SaveChanges();
        }
    }
}
