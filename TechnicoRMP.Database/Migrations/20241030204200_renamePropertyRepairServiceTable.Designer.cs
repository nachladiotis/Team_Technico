﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechnicoRMP.Database.DataAccess;

#nullable disable

namespace TechnicoRMP.Migrations;

[DbContext(typeof(DataStore))]
[Migration("20241030204200_renamePropertyRepairServiceTable")]
partial class renamePropertyRepairServiceTable
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("TechnicoRMP.Models.PropertyItem", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                b.Property<string>("Address")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("E9Number")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("EnPropertyType")
                    .HasColumnType("int");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<int>("YearOfConstruction")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("PropertyItems");
            });

        modelBuilder.Entity("TechnicoRMP.Models.PropertyOwnership", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                b.Property<long>("PropertyItemId")
                    .HasColumnType("bigint");

                b.Property<long>("PropertyOwnerId")
                    .HasColumnType("bigint");

                b.HasKey("Id");

                b.HasIndex("PropertyItemId");

                b.HasIndex("PropertyOwnerId");

                b.ToTable("PropertyOwnerships");
            });

        modelBuilder.Entity("TechnicoRMP.Models.PropertyRepair", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                b.Property<string>("Address")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<decimal>("Cost")
                    .HasColumnType("decimal(18,2)");

                b.Property<DateTime?>("Date")
                    .HasColumnType("datetime2");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<int>("RepairStatus")
                    .HasColumnType("int");

                b.Property<long>("RepairerId")
                    .HasColumnType("bigint");

                b.Property<int>("TypeOfRepair")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("RepairerId");

                b.ToTable("PropertyRepairs");
            });

        modelBuilder.Entity("TechnicoRMP.Models.User", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                b.Property<string>("Address")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Password")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Surname")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("TypeOfUser")
                    .HasColumnType("int");

                b.Property<string>("VatNumber")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("VatNumber")
                    .IsUnique();

                b.ToTable("Users");
            });

        modelBuilder.Entity("TechnicoRMP.Models.PropertyOwnership", b =>
            {
                b.HasOne("TechnicoRMP.Models.PropertyItem", "PropertyItem")
                    .WithMany("PropertyOwnerships")
                    .HasForeignKey("PropertyItemId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("TechnicoRMP.Models.User", "PropertyOwner")
                    .WithMany("PropertyOwnerships")
                    .HasForeignKey("PropertyOwnerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("PropertyItem");

                b.Navigation("PropertyOwner");
            });

        modelBuilder.Entity("TechnicoRMP.Models.PropertyRepair", b =>
            {
                b.HasOne("TechnicoRMP.Models.User", "Repairer")
                    .WithMany("RepairsHistory")
                    .HasForeignKey("RepairerId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Repairer");
            });

        modelBuilder.Entity("TechnicoRMP.Models.PropertyItem", b =>
            {
                b.Navigation("PropertyOwnerships");
            });

        modelBuilder.Entity("TechnicoRMP.Models.User", b =>
            {
                b.Navigation("PropertyOwnerships");

                b.Navigation("RepairsHistory");
            });
#pragma warning restore 612, 618
    }
}
