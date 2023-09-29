﻿// <auto-generated />
using System;
using DeliveryService.Context.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeliveryService.Context.Migrations
{
    [DbContext(typeof(DeliveryServiceContext))]
    [Migration("20230928133546_M3TicketChanges")]
    partial class M3TicketChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DeliveryService.Context.Models.Courier", b =>
                {
                    b.Property<int>("CourierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourierId"));

                    b.HasKey("CourierId");

                    b.ToTable("Couriers");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Package", b =>
                {
                    b.Property<int>("PackageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PackageId"));

                    b.Property<int?>("CourierId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCollected")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateDelivered")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RequestedDeliveryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.HasKey("PackageId");

                    b.HasIndex("CourierId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("TicketId");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketId"));

                    b.Property<string>("Commentary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Completed")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CourierId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("TicketId");

                    b.HasIndex("CourierId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Package", b =>
                {
                    b.HasOne("DeliveryService.Context.Models.Courier", "Courier")
                        .WithMany("Packages")
                        .HasForeignKey("CourierId");

                    b.HasOne("DeliveryService.Context.Models.Customer", "Customer")
                        .WithMany("Packages")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryService.Context.Models.Ticket", "Ticket")
                        .WithMany("Packages")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Courier");

                    b.Navigation("Customer");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Ticket", b =>
                {
                    b.HasOne("DeliveryService.Context.Models.Courier", "Courier")
                        .WithMany("Tickets")
                        .HasForeignKey("CourierId");

                    b.HasOne("DeliveryService.Context.Models.Customer", "Customer")
                        .WithMany("Tickets")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Courier");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Courier", b =>
                {
                    b.Navigation("Packages");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Customer", b =>
                {
                    b.Navigation("Packages");

                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("DeliveryService.Context.Models.Ticket", b =>
                {
                    b.Navigation("Packages");
                });
#pragma warning restore 612, 618
        }
    }
}
