﻿// <auto-generated />
using System;
using Houseware.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HousewareWebAPI.Migrations
{
    [DbContext(typeof(HousewareContext))]
    [Migration("20220603031637_AddAnyPropertyProduct")]
    partial class AddAnyPropertyProduct
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.16")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Address", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<string>("ProvinceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Type")
                        .HasColumnType("bit");

                    b.Property<string>("WardId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WardName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Cart", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.HasKey("CustomerId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Category", b =>
                {
                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Advantage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClassificationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Enable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slogan")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sort")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2147483647);

                    b.Property<string>("Video")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.HasIndex("ClassificationId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Classification", b =>
                {
                    b.Property<string>("ClassificationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Enable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("ImageBanner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageMenu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sort")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2147483647);

                    b.Property<string>("Story")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClassificationId");

                    b.ToTable("Classifications");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Customer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DefaultAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerifyEmail")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("N");

                    b.Property<string>("VerifyPhone")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("N");

                    b.HasKey("CustomerId");

                    b.HasIndex("DefaultAddressId")
                        .IsUnique()
                        .HasFilter("[DefaultAddressId] IS NOT NULL");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("Phone")
                        .IsUnique()
                        .HasFilter("[Phone] IS NOT NULL");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Order", b =>
                {
                    b.Property<Guid>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LabelId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

                    b.Property<string>("PaymentType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("COD");

                    b.Property<bool>("StatusPaid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("AddressId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("StoreId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.OrderDetail", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Product", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

                    b.Property<string>("Design")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Enable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<long>("Height")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<string>("Highlights")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Length")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<DateTime>("ModifyDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Overview")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Performance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<int>("Sort")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2147483647);

                    b.Property<long>("View")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<long>("Weight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<long>("Width")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.ProductSpecification", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SpecificationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductId", "SpecificationId");

                    b.HasIndex("SpecificationId");

                    b.ToTable("ProductSpecifications");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Specification", b =>
                {
                    b.Property<string>("SpecificationId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sort")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2147483647);

                    b.HasKey("SpecificationId");

                    b.ToTable("Specifications");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Store", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DistrictId")
                        .HasColumnType("int");

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.Property<string>("ProvinceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.Property<string>("WardId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WardName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StoreId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Stored", b =>
                {
                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.HasKey("StoreId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("Storeds");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Address", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Customer", "Customer")
                        .WithMany("Addresses")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Cart", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Customer", "Customer")
                        .WithMany("Carts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousewareWebAPI.Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Category", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Classification", "Classification")
                        .WithMany("Categories")
                        .HasForeignKey("ClassificationId");

                    b.Navigation("Classification");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Customer", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Address", "DefaultAddress")
                        .WithOne("DefaultCustomer")
                        .HasForeignKey("HousewareWebAPI.Data.Entities.Customer", "DefaultAddressId");

                    b.Navigation("DefaultAddress");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Order", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Address", "Address")
                        .WithMany("Orders")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("HousewareWebAPI.Data.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId");

                    b.HasOne("HousewareWebAPI.Data.Entities.Store", "Store")
                        .WithMany("Orders")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Customer");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.OrderDetail", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousewareWebAPI.Data.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Product", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.ProductSpecification", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Product", "Product")
                        .WithMany("ProductSpecifications")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousewareWebAPI.Data.Entities.Specification", "Specification")
                        .WithMany("ProductSpecifications")
                        .HasForeignKey("SpecificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Specification");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Stored", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Product", "Product")
                        .WithMany("Storeds")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HousewareWebAPI.Data.Entities.Store", "Store")
                        .WithMany("Storeds")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Address", b =>
                {
                    b.Navigation("DefaultCustomer");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Classification", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Customer", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Carts");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Order", b =>
                {
                    b.Navigation("OrderDetails");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Product", b =>
                {
                    b.Navigation("ProductSpecifications");

                    b.Navigation("Storeds");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Specification", b =>
                {
                    b.Navigation("ProductSpecifications");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Store", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Storeds");
                });
#pragma warning restore 612, 618
        }
    }
}
