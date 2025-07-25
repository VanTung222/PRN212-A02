using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FUMiniHotelSystem.DAL.Models;

public partial class FUMiniHotelManagementContext : DbContext
{
    public FUMiniHotelManagementContext()
    {
    }

    public FUMiniHotelManagementContext(DbContextOptions<FUMiniHotelManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookingDetail> BookingDetails { get; set; }

    public virtual DbSet<BookingReservation> BookingReservations { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<RoomInformation> RoomInformations { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());
    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];

        return strConn;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.HasOne(d => d.BookingReservation).WithMany(p => p.BookingDetails).HasConstraintName("FK_BookingDetail_BookingReservation");

            entity.HasOne(d => d.Room).WithMany(p => p.BookingDetails).HasConstraintName("FK_BookingDetail_RoomInformation");
        });

        modelBuilder.Entity<BookingReservation>(entity =>
        {
            entity.Property(e => e.BookingReservationId).ValueGeneratedNever();

            entity.HasOne(d => d.Customer).WithMany(p => p.BookingReservations).HasConstraintName("FK_BookingReservation_Customer");
        });

        modelBuilder.Entity<RoomInformation>(entity =>
        {
            entity.HasOne(d => d.RoomType).WithMany(p => p.RoomInformations).HasConstraintName("FK_RoomInformation_RoomType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
