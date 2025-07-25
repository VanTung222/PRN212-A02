using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.DAL.Models;

[Table("Customer")]
[Index("EmailAddress", Name = "UQ__Customer__49A147405EE8AC4C", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("CustomerID")]
    public int CustomerId { get; set; }

    [StringLength(50)]
    public string? CustomerFullName { get; set; }

    [StringLength(12)]
    public string? Telephone { get; set; }

    [StringLength(50)]
    public string EmailAddress { get; set; } = null!;

    public DateOnly? CustomerBirthday { get; set; }

    public byte? CustomerStatus { get; set; }

    [StringLength(50)]
    public string? Password { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<BookingReservation> BookingReservations { get; set; } = new List<BookingReservation>();
}
