using CleverBit.Task1.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleverBit.Task1.Data
{
    public class CleverBitDbContext : DbContext
    {
        public CleverBitDbContext(DbContextOptions<CleverBitDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Region>()
                .HasOne(b => b.Parent)
                .WithMany(a => a.Children)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
