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
    [Migration("20220502170735_CreateProductSpecficationDB")]
    partial class CreateProductSpecficationDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.16")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .HasDefaultValueSql("GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

                    b.Property<string>("Design")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Enable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Highlights")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE() AT TIME ZONE 'N. Central Asia Standard Time'");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Overview")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Performance")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("Sort")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2147483647);

                    b.Property<int>("View")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

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

                    b.ToTable("ProductSpecification");
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

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Category", b =>
                {
                    b.HasOne("HousewareWebAPI.Data.Entities.Classification", "Classification")
                        .WithMany("Categories")
                        .HasForeignKey("ClassificationId");

                    b.Navigation("Classification");
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

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Classification", b =>
                {
                    b.Navigation("Categories");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Product", b =>
                {
                    b.Navigation("ProductSpecifications");
                });

            modelBuilder.Entity("HousewareWebAPI.Data.Entities.Specification", b =>
                {
                    b.Navigation("ProductSpecifications");
                });
#pragma warning restore 612, 618
        }
    }
}
