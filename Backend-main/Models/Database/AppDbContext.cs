using Microsoft.EntityFrameworkCore;

namespace Backend.Models.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Fragen> Fragen { get; set; }
        public DbSet<Antworten> Antworten { get; set; }
        public DbSet<Datei> Datei { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Fragen>()
                .HasOne(f => f.Datei)
                .WithMany(d => d.Fragen)
                .HasForeignKey(f => f.DateiId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Antworten>()
                .HasOne(a => a.Fragen)
                .WithMany(f => f.Antworten)
                .HasForeignKey(a => a.FragenId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
