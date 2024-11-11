using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnicoRMP.Models;

namespace TechnicoRMP.Database.DataAccess;

public class DataStore : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyItem> PropertyItems { get; set; }
    public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
    public DbSet<PropertyRepair> PropertyRepairs { get; set; }

    public DataStore(DbContextOptions<DataStore> options) : base(options)
    {
    }

    //public void ConfigureServices(IServiceCollection services)
    //{
    //    services.AddDbContext<DataStore>(options =>
    //        options.UseSqlServer("Data Source=(local);Initial Catalog=TechnicoDb;Integrated Security=True;TrustServerCertificate=True;"));
    //}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<User>()
            .HasIndex(p => p.VatNumber)
            .IsUnique();
    }

}
