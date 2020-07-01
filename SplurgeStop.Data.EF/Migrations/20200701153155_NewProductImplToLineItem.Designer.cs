﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SplurgeStop.Data.EF;

namespace SplurgeStop.Data.EF.Migrations
{
    [DbContext(typeof(SplurgeStopDbContext))]
    [Migration("20200701153155_NewProductImplToLineItem")]
    partial class NewProductImplToLineItem
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SplurgeStop.Domain.CityProfile.City", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("SplurgeStop.Domain.CountryProfile.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("SplurgeStop.Domain.LocationProfile.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("SplurgeStop.Domain.ProductProfile.BrandProfile.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("SplurgeStop.Domain.ProductProfile.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BrandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<Guid?>("ProductTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SplurgeStop.Domain.ProductProfile.TypeProfile.ProductType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("SplurgeStop.Domain.PurchaseTransaction.LineItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .HasColumnName("Price")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PurchaseTransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TEMPProductName")
                        .HasColumnName("Product")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("PurchaseTransactionId");

                    b.ToTable("LineItem");
                });

            modelBuilder.Entity("SplurgeStop.Domain.PurchaseTransaction.PurchaseTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("Date");

                    b.Property<Guid?>("StoreId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("Purchases");
                });

            modelBuilder.Entity("SplurgeStop.Domain.StoreProfile.Store", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("SplurgeStop.Domain.LocationProfile.Location", b =>
                {
                    b.HasOne("SplurgeStop.Domain.CityProfile.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("SplurgeStop.Domain.CountryProfile.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("SplurgeStop.Domain.ProductProfile.Product", b =>
                {
                    b.HasOne("SplurgeStop.Domain.ProductProfile.BrandProfile.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId");

                    b.HasOne("SplurgeStop.Domain.ProductProfile.TypeProfile.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId");
                });

            modelBuilder.Entity("SplurgeStop.Domain.PurchaseTransaction.LineItem", b =>
                {
                    b.HasOne("SplurgeStop.Domain.ProductProfile.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.HasOne("SplurgeStop.Domain.PurchaseTransaction.PurchaseTransaction", "PurchaseTransaction")
                        .WithMany("LineItems")
                        .HasForeignKey("PurchaseTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SplurgeStop.Domain.PurchaseTransaction.PurchaseTransaction", b =>
                {
                    b.HasOne("SplurgeStop.Domain.StoreProfile.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId");
                });

            modelBuilder.Entity("SplurgeStop.Domain.StoreProfile.Store", b =>
                {
                    b.HasOne("SplurgeStop.Domain.LocationProfile.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });
#pragma warning restore 612, 618
        }
    }
}
