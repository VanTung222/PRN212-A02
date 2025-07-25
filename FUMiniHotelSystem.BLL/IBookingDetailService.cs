using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.BLL
{
    public interface IBookingDetailService
    {
        Task<IEnumerable<BookingDetail>> GetAllBookingDetailsAsync();

        Task<BookingDetail> GetBookingDetailAsync(int bookingReservationId, int roomId);

        Task AddBookingDetailAsync(BookingDetail detail);

        Task UpdateBookingDetailAsync(BookingDetail detail);

        Task DeleteBookingDetailAsync(int bookingReservationId, int roomId);

        Task<IEnumerable<BookingDetail>> GetByBookingIdAsync(int bookingReservationId);
    }
}