using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Models;

namespace TechnicoRMP.Database.DataAccess;

public class DataStore : DbContext
{
    public DataStore(DbContextOptions<DataStore> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyItem> PropertyItems { get; set; }
    public DbSet<PropertyOwnership> PropertyOwnerships { get; set; }
    public DbSet<PropertyRepair> PropertyRepairs { get; set; }
  
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

       // modelBuilder.Entity<User>().HasData(
       //    new User()
       //    {
       //        Id=1,
       //        Email = "admin@admin.com",
       //        Name = "Admin",
       //        Password = "admin",  // ΠΡΟΣΟΧΗ: Κρυπτογράφησε τον κωδικό στην πραγματική εφαρμογή
       //        Surname = "admin",
       //        VatNumber = "122431234"
       //    }
       //);

        modelBuilder
            .Entity<User>()
            .HasIndex(p => p.VatNumber)
            .IsUnique();

        //modelBuilder.Entity<User>().HasData(
        //    new User
        //    {
        //        Name = "John",
        //        Surname = "Doe",
        //        VatNumber = "123456789",
        //        Address = "123 Main St",
        //        PhoneNumber = "555-1234",
        //        Email = "john.doe@example.com",
        //        Password = "password123",
        //        TypeOfUser = EnUserType.Customer
        //    }
        //);
        //modelBuilder.Entity<PropertyItem>().HasData(
        //    new PropertyItem
        //    {
        //        E9Number = "E912345678",
        //        Address = "123 Main Street",
        //        YearOfConstruction = 1995,
        //        EnPropertyType = EnPropertyType.Apartment,
        //        IsActive = true
        //    },
        //    new PropertyItem
        //    {
        //        E9Number = "E987654321",
        //        Address = "456 Another Street",
        //        YearOfConstruction = 2005,
        //        EnPropertyType = EnPropertyType.Apartment,
        //        IsActive = true
        //    }
        //);
        //modelBuilder.Entity<PropertyRepair>().HasData(
        //    new PropertyRepair
        //    {
        //        Date = new DateTime(2023, 10, 5),
        //        TypeOfRepair = EnTypeOfRepair.Plumbing,
        //        Address = "123 Main Street",
        //        RepairStatus = EnRepairStatus.Inprogress,
        //        Cost = 150.00m,
        //        RepairerId = 1,
        //        IsActive = true
        //    },
        //    new PropertyRepair
        //    {
        //        Date = new DateTime(2023, 11, 3),
        //        TypeOfRepair = EnTypeOfRepair.Painting,
        //        Address = "456 Another Street",
        //        RepairStatus = EnRepairStatus.Pending,
        //        Cost = 300.00m,
        //        RepairerId = 2,
        //        IsActive = true
        //    }
        //);
    }

}