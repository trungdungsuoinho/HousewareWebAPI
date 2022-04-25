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
    [Migration("20220425193635_CreateCategoryDB")]
    partial class CreateCategoryDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.16")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Houseware.WebAPI.Entities.Category", b =>
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

                    b.Property<int?>("Sort")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2147483647);

                    b.HasKey("CategoryId");

                    b.HasIndex("ClassificationId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Houseware.WebAPI.Entities.Classification", b =>
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

                    b.Property<int?>("Sort")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(2147483647);

                    b.Property<string>("Story")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClassificationId");

                    b.ToTable("Classifications");
                });

            modelBuilder.Entity("Houseware.WebAPI.Entities.Category", b =>
                {
                    b.HasOne("Houseware.WebAPI.Entities.Classification", "Classification")
                        .WithMany("Categories")
                        .HasForeignKey("ClassificationId");

                    b.Navigation("Classification");
                });

            modelBuilder.Entity("Houseware.WebAPI.Entities.Classification", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
