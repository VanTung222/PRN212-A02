using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.DAL.Models;

[Table("RoomType")]
public partial class RoomType
{
    [Key]
    [Column("RoomTypeID")]
    public int RoomTypeId { get; set; }

    [StringLength(50)]
    public string RoomTypeName { get; set; } = null!;

    [StringLength(250)]
    public string? TypeDescription { get; set; }

    [StringLength(250)]
    public string? TypeNote { get; set; }

    [InverseProperty("RoomType")]
    public virtual ICollection<RoomInformation> RoomInformations { get; set; } = new List<RoomInformation>();
}
