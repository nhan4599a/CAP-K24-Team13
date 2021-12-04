﻿// <auto-generated />
using System;
using DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabaseAccessor.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211204085816_UpdateDbForCategorySoftDelete")]
    partial class UpdateDbForCategorySoftDelete
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DatabaseAccessor.Model.ShopCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDisabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<int>("Special")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShopCategories");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopInterface", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Option")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShopInterfaces");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopProduct", b =>
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

                    b.ToTable("ShopProducts");

                    b.HasCheckConstraint("CK_ShopProducts_Price", "[Price] >= 0");

                    b.HasCheckConstraint("CK_ShopProducts_Quantity", "[Quantity] >= 1");

                    b.HasCheckConstraint("CK_ShopProducts_Discount", "[Discount] between 0 and 100");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopProduct", b =>
                {
                    b.HasOne("DatabaseAccessor.Model.ShopCategory", "Category")
                        .WithMany("ShopProducts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopCategory", b =>
                {
                    b.Navigation("ShopProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
