using Microsoft.EntityFrameworkCore;
using TaskTracker.Api.Models;

namespace TaskTracker.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}
