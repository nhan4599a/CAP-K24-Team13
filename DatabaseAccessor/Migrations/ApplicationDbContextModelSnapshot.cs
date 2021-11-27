﻿// <auto-generated />
using DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DatabaseAccessor.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DatabaseAccessor.Model.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopProductId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ShopProductId")
                        .IsUnique()
                        .HasFilter("[ShopProductId] IS NOT NULL");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<int>("Special")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ShopCategories");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image10")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image11")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image12")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image6")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image7")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image8")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image9")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopInterfaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopInterfaceId")
                        .IsUnique();

                    b.ToTable("ShopImages");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopInterface", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
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
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<bool>("IsDisabled")
                        .HasColumnType("bit");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("ShopProducts");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ProductImage", b =>
                {
                    b.HasOne("DatabaseAccessor.Model.ShopProduct", "Product")
                        .WithOne("ImageSet")
                        .HasForeignKey("DatabaseAccessor.Model.ProductImage", "ShopProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopImage", b =>
                {
                    b.HasOne("DatabaseAccessor.Model.ShopInterface", "ShopInterface")
                        .WithOne("ShopImage")
                        .HasForeignKey("DatabaseAccessor.Model.ShopImage", "ShopInterfaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShopInterface");
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

            modelBuilder.Entity("DatabaseAccessor.Model.ShopInterface", b =>
                {
                    b.Navigation("ShopImage");
                });

            modelBuilder.Entity("DatabaseAccessor.Model.ShopProduct", b =>
                {
                    b.Navigation("ImageSet");
                });
#pragma warning restore 612, 618
        }
    }
}