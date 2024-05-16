
using Microsoft.EntityFrameworkCore;
using TaskCreationService.Models;

namespace TaskCreationService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<UserTask> UserTasks { get; set; }
       /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
        }*/
    }
}
