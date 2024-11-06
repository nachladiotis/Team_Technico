using Microsoft.EntityFrameworkCore;

namespace TechnicoRMP.Database.DataAccess;

public class DataStore : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyItem> PropertyItems { get; set; }
    public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
    public DbSet<PropertyRepair> PropertyRepairs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = "Data Source=(local);Initial Catalog=TechnicoDb; Integrated Security = True;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasIndex(p => p.VatNumber)
            .IsUnique();
    }

}
