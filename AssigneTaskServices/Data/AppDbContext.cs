
using AssigneTaskServices.Models;
using Microsoft.EntityFrameworkCore;

namespace AssigneTaskServices.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<AssignedUserTask> AssignedTasks { get; set; }
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }*/
    }
}
