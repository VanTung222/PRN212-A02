using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUMiniHotelSystem.BLL
{
    public class BookingDetailService : IBookingDetailService
    {
        private readonly IBookingDetailRepository _bookingDetailRepo;

        public BookingDetailService(IBookingDetailRepository bookingDetailRepo)
        {
            _bookingDetailRepo = bookingDetailRepo;
        }

        public async Task AddBookingDetailAsync(BookingDetail detail)
        {
            await _bookingDetailRepo.AddAsync(detail);
        }

        public async Task DeleteBookingDetailAsync(int bookingReservationId, int roomId)
        {
            await _bookingDetailRepo.DeleteAsync(new object[] { bookingReservationId, roomId });
        }

        public async Task<IEnumerable<BookingDetail>> GetAllBookingDetailsAsync()
        {
            return await _bookingDetailRepo.GetAllAsync();
        }

        public async Task<BookingDetail> GetBookingDetailAsync(int bookingReservationId, int roomId)
        {
            return await _bookingDetailRepo.GetByIdAsync(new object[] { bookingReservationId, roomId });
        }

        public async Task<IEnumerable<BookingDetail>> GetByBookingIdAsync(int bookingReservationId)
        {
            return await _bookingDetailRepo.GetByBookingIdAsync(bookingReservationId);
        }

        public async Task UpdateBookingDetailAsync(BookingDetail detail)
        {
            await _bookingDetailRepo.UpdateAsync(detail);
        }
    }
}
