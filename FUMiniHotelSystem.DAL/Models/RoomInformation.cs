using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.DAL.Models;

[Table("RoomInformation")]
public partial class RoomInformation
{
    [Key]
    [Column("RoomID")]
    public int RoomId { get; set; }

    [StringLength(50)]
    public string RoomNumber { get; set; } = null!;

    [StringLength(220)]
    public string? RoomDetailDescription { get; set; }

    public int? RoomMaxCapacity { get; set; }

    [Column("RoomTypeID")]
    public int RoomTypeId { get; set; }

    public byte? RoomStatus { get; set; }

    [Column(TypeName = "money")]
    public decimal? RoomPricePerDay { get; set; }

    [InverseProperty("Room")]
    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    [ForeignKey("RoomTypeId")]
    [InverseProperty("RoomInformations")]
    public virtual RoomType RoomType { get; set; } = null!;
}
