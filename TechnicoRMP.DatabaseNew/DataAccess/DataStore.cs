using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Models;
using TechnicoRMP.Models.Logs;
namespace TechnicoRMP.Database.DataAccess;

public class DataStore : DbContext
{
    public DataStore(DbContextOptions<DataStore> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyItem> PropertyItems { get; set; }
    public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
    public DbSet<PropertyRepair> PropertyRepairs { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasIndex(p => p.VatNumber)
            .IsUnique();

        modelBuilder
            .Entity<PropertyItem>()
            .HasIndex(p => p.E9Number)
            .IsUnique();
    }

}