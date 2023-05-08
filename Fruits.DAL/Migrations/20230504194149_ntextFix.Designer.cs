﻿// <auto-generated />
using System;
using Fruits.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fruits.DAL.Migrations
{
    [DbContext(typeof(FruitDbContext))]
    [Migration("20230504194149_ntextFix")]
    partial class ntextFix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Fruits.Domain.DB.FruitsCatalog", b =>
                {
                    b.Property<int>("IdFruitsCatalog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFruitsCatalog"));

                    b.Property<string>("Class")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sort")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdFruitsCatalog");

                    b.ToTable("FruitsCatalog");
                });

            modelBuilder.Entity("Fruits.Domain.DB.PriceCatalog", b =>
                {
                    b.Property<int>("IdPriceCatalog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPriceCatalog"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdFruitsCatalog")
                        .HasColumnType("int");

                    b.Property<int>("IdProviderCatalog")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("IdPriceCatalog");

                    b.HasIndex("IdFruitsCatalog");

                    b.HasIndex("IdProviderCatalog");

                    b.ToTable("PriceCatalog");
                });

            modelBuilder.Entity("Fruits.Domain.DB.ProvidersCatalog", b =>
                {
                    b.Property<int>("IdProviderCatalog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProviderCatalog"));

                    b.Property<string>("NameProvider")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdProviderCatalog");

                    b.ToTable("ProvidersCatalog");
                });

            modelBuilder.Entity("Fruits.Domain.DB.Stock", b =>
                {
                    b.Property<int>("IdStock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStock"));

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdProviderCatalog")
                        .HasColumnType("int");

                    b.HasKey("IdStock");

                    b.HasIndex("IdProviderCatalog");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("Fruits.Domain.DB.StockFruits", b =>
                {
                    b.Property<int>("IdStockFruits")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdStockFruits"));

                    b.Property<int>("IdFruitsCatalog")
                        .HasColumnType("int");

                    b.Property<int>("IdStock")
                        .HasColumnType("int");

                    b.Property<int>("Mass")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("IdStockFruits");

                    b.HasIndex("IdFruitsCatalog");

                    b.HasIndex("IdStock");

                    b.ToTable("StockFruits");
                });

            modelBuilder.Entity("Fruits.Domain.DB.PriceCatalog", b =>
                {
                    b.HasOne("Fruits.Domain.DB.FruitsCatalog", "FruitsCatalog")
                        .WithMany("PriceCatalog")
                        .HasForeignKey("IdFruitsCatalog")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fruits.Domain.DB.ProvidersCatalog", "ProvidersCatalog")
                        .WithMany("PriceCatalog")
                        .HasForeignKey("IdProviderCatalog")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FruitsCatalog");

                    b.Navigation("ProvidersCatalog");
                });

            modelBuilder.Entity("Fruits.Domain.DB.Stock", b =>
                {
                    b.HasOne("Fruits.Domain.DB.ProvidersCatalog", "ProvidersCatalog")
                        .WithMany("Stock")
                        .HasForeignKey("IdProviderCatalog")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ProvidersCatalog");
                });

            modelBuilder.Entity("Fruits.Domain.DB.StockFruits", b =>
                {
                    b.HasOne("Fruits.Domain.DB.FruitsCatalog", "FruitsCatalog")
                        .WithMany("StockFruits")
                        .HasForeignKey("IdFruitsCatalog")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Fruits.Domain.DB.Stock", "Stock")
                        .WithMany("StockFruits")
                        .HasForeignKey("IdStock")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FruitsCatalog");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Fruits.Domain.DB.FruitsCatalog", b =>
                {
                    b.Navigation("PriceCatalog");

                    b.Navigation("StockFruits");
                });

            modelBuilder.Entity("Fruits.Domain.DB.ProvidersCatalog", b =>
                {
                    b.Navigation("PriceCatalog");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("Fruits.Domain.DB.Stock", b =>
                {
                    b.Navigation("StockFruits");
                });
#pragma warning restore 612, 618
        }
    }
}
