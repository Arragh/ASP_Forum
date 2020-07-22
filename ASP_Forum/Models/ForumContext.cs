using Microsoft.EntityFrameworkCore;

namespace ASP_Forum.Models
{
    public class ForumContext : DbContext
    {
        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Reply> Replies { get; set; }

        public ForumContext(DbContextOptions<ForumContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
