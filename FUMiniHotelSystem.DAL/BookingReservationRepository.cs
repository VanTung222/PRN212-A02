using Microsoft.EntityFrameworkCore;
using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        private readonly FUMiniHotelManagementContext _context;

        public BookingReservationRepository(FUMiniHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingReservation>> GetAllAsync()
            => await _context.BookingReservations.Include(b => b.Customer).ToListAsync();

        public async Task<int> GetMaxBookingReservationIdAsync()
        {
            return await _context.BookingReservations.MaxAsync(x => (int?)x.BookingReservationId) ?? 0;
        }

        public async Task<BookingReservation> GetByIdAsync(object id)
            => await _context.BookingReservations.Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.BookingReservationId == (int)id);

        public async Task AddAsync(BookingReservation entity)
        {
            // Generate the next available ID if not provided
            if (entity.BookingReservationId == 0)
            {
                var maxId = await GetMaxBookingReservationIdAsync();
                entity.BookingReservationId = maxId + 1;
            }
            
            await _context.BookingReservations.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookingReservation entity)
        {
            _context.BookingReservations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var booking = await _context.BookingReservations.FindAsync(id);
            if (booking != null)
            {
                _context.BookingReservations.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<BookingReservation>> GetByCustomerAsync(int customerId)
            => await _context.BookingReservations
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

        public async Task<IEnumerable<BookingReservation>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var startDateOnly = DateOnly.FromDateTime(start);
            var endDateOnly = DateOnly.FromDateTime(end);

            return await _context.BookingReservations
                .Where(b => b.BookingDate.HasValue
                         && b.BookingDate.Value >= startDateOnly
                         && b.BookingDate.Value <= endDateOnly)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var booking = await _context.BookingReservations.FindAsync(bookingId);
            if (booking != null)
            {
                booking.BookingStatus = 0; // 0 = Cancelled (tùy thiết kế)
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}