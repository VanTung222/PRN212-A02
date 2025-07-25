using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.DAL.Models;

[PrimaryKey("BookingReservationId", "RoomId")]
[Table("BookingDetail")]
public partial class BookingDetail
{
    [Key]
    [Column("BookingReservationID")]
    public int BookingReservationId { get; set; }

    [Key]
    [Column("RoomID")]
    public int RoomId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [Column(TypeName = "money")]
    public decimal? ActualPrice { get; set; }

    [ForeignKey("BookingReservationId")]
    [InverseProperty("BookingDetails")]
    public virtual BookingReservation BookingReservation { get; set; } = null!;

    [ForeignKey("RoomId")]
    [InverseProperty("BookingDetails")]
    public virtual RoomInformation Room { get; set; } = null!;
}
