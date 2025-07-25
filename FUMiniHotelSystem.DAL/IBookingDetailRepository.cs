
using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public interface IBookingDetailRepository : IRepository<BookingDetail>
    {
        Task<IEnumerable<BookingDetail>> GetByBookingIdAsync(int bookingReservationId);
    }
}