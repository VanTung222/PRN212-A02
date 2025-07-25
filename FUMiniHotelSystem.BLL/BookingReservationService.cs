using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.DAL;
using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.BLL
{
    public class BookingReservationService : IBookingReservationService
    {
        private readonly IBookingReservationRepository _bookingRepo;

        public BookingReservationService(IBookingReservationRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;
        }
        public async Task<int> GetMaxBookingReservationIdAsync()
        {
            return await _bookingRepo.GetMaxBookingReservationIdAsync();
        }

        public async Task<IEnumerable<BookingReservation>> GetAllBookingReservationsAsync()
            => await _bookingRepo.GetAllAsync();

        public async Task<BookingReservation> GetBookingReservationByIdAsync(int id)
            => await _bookingRepo.GetByIdAsync(id);

        public async Task<BookingReservation> AddBookingReservationAsync(BookingReservation booking)
        {
            await _bookingRepo.AddAsync(booking);
            return booking; // Return the booking with the generated ID
        }

        public async Task UpdateBookingReservationAsync(BookingReservation booking)
            => await _bookingRepo.UpdateAsync(booking);

        public async Task DeleteBookingReservationAsync(int id)
            => await _bookingRepo.DeleteAsync(id);

        public async Task<IEnumerable<BookingReservation>> GetByCustomerAsync(int customerId)
            => await _bookingRepo.GetByCustomerAsync(customerId);

        public async Task<IEnumerable<BookingReservation>> GetByDateRangeAsync(System.DateTime start, System.DateTime end)
            => await _bookingRepo.GetByDateRangeAsync(start, end);

        public async Task<bool> CancelBookingAsync(int bookingId)
            => await _bookingRepo.CancelBookingAsync(bookingId);
    }
}