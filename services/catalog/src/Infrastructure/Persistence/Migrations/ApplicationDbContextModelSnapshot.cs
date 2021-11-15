﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecommendCoffee.Catalog.Infrastructure.Persistence;

#nullable disable

namespace RecommendCoffee.Catalog.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ProductEvents", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sequence")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Sequence"), 1L, 1);

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("EventId");

                    b.HasIndex("AggregateId");

                    b.ToTable("ProductEvents");
                });

            modelBuilder.Entity("RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.ProductInformation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.ProductInformation", b =>
                {
                    b.OwnsMany("RecommendCoffee.Catalog.Domain.Projections.ProductInfoProjection.ProductVariantInformation", "ProductVariants", b1 =>
                        {
                            b1.Property<Guid>("ProductInformationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<decimal>("UnitPrice")
                                .HasPrecision(5, 2)
                                .HasColumnType("decimal(5,2)");

                            b1.Property<int>("Weight")
                                .HasColumnType("int");

                            b1.HasKey("ProductInformationId", "Id");

                            b1.ToTable("ProductVariantInformation");

                            b1.WithOwner()
                                .HasForeignKey("ProductInformationId");
                        });

                    b.Navigation("ProductVariants");
                });
#pragma warning restore 612, 618
        }
    }
}
