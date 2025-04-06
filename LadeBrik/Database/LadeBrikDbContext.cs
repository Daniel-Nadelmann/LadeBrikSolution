using LadeBrik.Models;
using Microsoft.EntityFrameworkCore;

namespace LadeBrik.Database;

public class LadeBrikDbContext : DbContext
{
    public LadeBrikDbContext(DbContextOptions<LadeBrikDbContext> options) : base(options)
    {
    }
    public DbSet<LadeBrikModel> LadeBriks => Set<LadeBrikModel>();
    public DbSet<LadeBrikArchiveModel> LadeBriksArchive => Set<LadeBrikArchiveModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LadeBrikModel>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<LadeBrikArchiveModel>()
            .HasKey(c => new { c.Id, c.ChangedAt });
    }
    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<LadeBrikModel>();
        var archiveEntries = new List<LadeBrikArchiveModel>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                var archiveEntry = new LadeBrikArchiveModel
                {
                    Id = entry.Entity.Id,
                    Active = entry.Entity.Active,
                    ChangedAt = DateTime.UtcNow,
                    Operation = entry.State == EntityState.Added ? Operation.Create :
                                entry.State == EntityState.Modified ? Operation.Update :
                                Operation.Delete
                };

                archiveEntries.Add(archiveEntry);
            }
        }

        LadeBriksArchive.AddRange(archiveEntries);

        return base.SaveChanges();
    }
}
