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
    [Migration("20200525135856_LineItem_notes")]
    partial class LineItem_notes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.Property<Guid?>("PurchaseTransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("SplurgeStop.Domain.PurchaseTransaction.LineItem", b =>
                {
                    b.HasOne("SplurgeStop.Domain.PurchaseTransaction.PurchaseTransaction", null)
                        .WithMany("LineItems")
                        .HasForeignKey("PurchaseTransactionId");
                });

            modelBuilder.Entity("SplurgeStop.Domain.PurchaseTransaction.PurchaseTransaction", b =>
                {
                    b.HasOne("SplurgeStop.Domain.StoreProfile.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId");
                });
#pragma warning restore 612, 618
        }
    }
}
