using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechnicoRMP.Models;

namespace TechnicoRMP.Database.DataAccess;

public class DataStore : DbContext
{
    public DataStore(DbContextOptions<DataStore> options) : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyItem> PropertyItems { get; set; }
    public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
    public DbSet<PropertyRepair> PropertyRepairs { get; set; }

   

    //public void ConfigureServices(IServiceCollection services)
    //{
    //    services.AddDbContext<DataStore>(options =>
    //        options.UseSqlServer("Data Source=(local);Initial Catalog=TechnicoDb;Integrated Security=True;TrustServerCertificate=True;"));


    //}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>().HasData(
           new User()
           {
               Id = 1,
               Email = "abcd@gmail.com",
               Name = "Ant",
               Password = "12",  // ΠΡΟΣΟΧΗ: Κρυπτογράφησε τον κωδικό στην πραγματική εφαρμογή
               Surname = "pl",
               VatNumber = "122431234"
           }
       );

        modelBuilder
            .Entity<User>()
            .HasIndex(p => p.VatNumber)
            .IsUnique();
    }

}