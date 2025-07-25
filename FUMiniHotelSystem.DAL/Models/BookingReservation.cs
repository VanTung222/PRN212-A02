using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.DAL.Models;

[Table("BookingReservation")]
public partial class BookingReservation
{
    [Key]
    [Column("BookingReservationID")]
    public int BookingReservationId { get; set; }

    public DateOnly? BookingDate { get; set; }

    [Column(TypeName = "money")]
    public decimal? TotalPrice { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    public byte? BookingStatus { get; set; }

    [InverseProperty("BookingReservation")]
    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    [ForeignKey("CustomerId")]
    [InverseProperty("BookingReservations")]
    public virtual Customer Customer { get; set; } = null!;
}
