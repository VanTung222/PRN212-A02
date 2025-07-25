using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public interface IBookingReservationRepository : IRepository<BookingReservation>
    {
        Task<IEnumerable<BookingReservation>> GetByCustomerAsync(int customerId);

        Task<IEnumerable<BookingReservation>> GetByDateRangeAsync(System.DateTime start, System.DateTime end);

        Task<int> GetMaxBookingReservationIdAsync();

        Task<bool> CancelBookingAsync(int bookingId);
    }
}