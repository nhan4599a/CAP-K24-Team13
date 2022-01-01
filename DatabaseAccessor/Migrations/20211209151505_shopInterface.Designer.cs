﻿// <auto-generated />
using System;
using DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatabaseAccessor.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211209151505_shopInterface")]
    partial class shopInterface
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DatabaseAccessor.Models.ShopCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDisabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<int>("Special")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryName");

                    b.ToTable("ShopCategories", (string)null);
                });

            modelBuilder.Entity("DatabaseAccessor.Models.ShopInterface", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<string>("ShopName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopPhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ShopId")
                        .IsUnique();

                    b.ToTable("ShopInterfaces", (string)null);
                });

            modelBuilder.Entity("DatabaseAccessor.Models.ShopProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDisabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductName");

                    b.ToTable("ShopProducts", (string)null);

                    b.HasCheckConstraint("CK_ShopProducts_Discount", "[Discount] between 0 and 100");

                    b.HasCheckConstraint("CK_ShopProducts_Price", "[Price] >= 0");

                    b.HasCheckConstraint("CK_ShopProducts_Quantity", "[Quantity] >= 1");
                });

            modelBuilder.Entity("DatabaseAccessor.Models.ShopProduct", b =>
                {
                    b.HasOne("DatabaseAccessor.Models.ShopCategory", "Category")
                        .WithMany("ShopProducts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DatabaseAccessor.Models.ShopCategory", b =>
                {
                    b.Navigation("ShopProducts");
                });
#pragma warning restore 612, 618
        }
    }
}