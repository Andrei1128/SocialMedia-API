﻿// <auto-generated />
using System;
using FirstProject_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FirstProject_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230512090537_AddedForeignKeyToVillaNumber")]
    partial class AddedForeignKeyToVillaNumber
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FirstProject_API.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Occupancy")
                        .HasColumnType("int");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<int>("Sqft")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenity = "",
                            CreatedDate = new DateTime(2023, 5, 12, 12, 5, 37, 47, DateTimeKind.Local).AddTicks(3717),
                            Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                            ImageUrl = "",
                            Name = "Royal Villa",
                            Occupancy = 5,
                            Rate = 200.0,
                            Sqft = 440,
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            Amenity = "",
                            CreatedDate = new DateTime(2023, 5, 12, 12, 5, 37, 47, DateTimeKind.Local).AddTicks(3769),
                            Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                            ImageUrl = "",
                            Name = "Beach Villa",
                            Occupancy = 10,
                            Rate = 100.0,
                            Sqft = 240,
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 3,
                            Amenity = "",
                            CreatedDate = new DateTime(2023, 5, 12, 12, 5, 37, 47, DateTimeKind.Local).AddTicks(3772),
                            Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                            ImageUrl = "",
                            Name = "New Villa",
                            Occupancy = 12,
                            Rate = 200.0,
                            Sqft = 340,
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 4,
                            Amenity = "",
                            CreatedDate = new DateTime(2023, 5, 12, 12, 5, 37, 47, DateTimeKind.Local).AddTicks(3774),
                            Details = "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.",
                            ImageUrl = "",
                            Name = "Old Villa",
                            Occupancy = 2,
                            Rate = 4.0,
                            Sqft = 140,
                            UpdatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("FirstProject_API.Models.VillaNumber", b =>
                {
                    b.Property<int>("VillaNo")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SpecialDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VillaId")
                        .HasColumnType("int");

                    b.HasKey("VillaNo");

                    b.HasIndex("VillaId");

                    b.ToTable("VillaNumbers");
                });

            modelBuilder.Entity("FirstProject_API.Models.VillaNumber", b =>
                {
                    b.HasOne("FirstProject_API.Models.Villa", "Villa")
                        .WithMany()
                        .HasForeignKey("VillaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Villa");
                });
#pragma warning restore 612, 618
        }
    }
}