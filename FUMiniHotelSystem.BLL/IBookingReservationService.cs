using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.BLL
{
    public interface IBookingReservationService
    {
        Task<IEnumerable<BookingReservation>> GetAllBookingReservationsAsync();

        Task<BookingReservation> GetBookingReservationByIdAsync(int id);

        Task<BookingReservation> AddBookingReservationAsync(BookingReservation booking);

        Task UpdateBookingReservationAsync(BookingReservation booking);

        Task DeleteBookingReservationAsync(int id);

        Task<IEnumerable<BookingReservation>> GetByCustomerAsync(int customerId);

        Task<IEnumerable<BookingReservation>> GetByDateRangeAsync(System.DateTime start, System.DateTime end);
        Task<int> GetMaxBookingReservationIdAsync();
        Task<bool> CancelBookingAsync(int bookingId);
    }
}