using Microsoft.EntityFrameworkCore;
using SummitStories.Api.Modules.Data.Models;
using System.Reflection.Emit;

namespace SummitStories.Api.Modules.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }

}
