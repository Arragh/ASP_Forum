using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_Forum.Models
{
    public class ForumContext : DbContext
    {
        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }

        public ForumContext(DbContextOptions<ForumContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
