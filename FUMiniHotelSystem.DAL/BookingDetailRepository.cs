using Microsoft.EntityFrameworkCore;
using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        private readonly FUMiniHotelManagementContext _context;

        public BookingDetailRepository(FUMiniHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingDetail>> GetAllAsync()
    => await _context.BookingDetails
        .Include(bd => bd.Room)
        .Include(bd => bd.BookingReservation)
            .ThenInclude(br => br.Customer)
        .ToListAsync();

        public async Task<IEnumerable<BookingDetail>> GetByIdAsync(int bookingReservationId)
    => await _context.BookingDetails
        .Include(bd => bd.Room)
        .Include(bd => bd.BookingReservation)
            .ThenInclude(br => br.Customer)
        .Where(bd => bd.BookingReservationId == bookingReservationId)
        .ToListAsync();


        public async Task AddAsync(BookingDetail entity)
        {
            await _context.BookingDetails.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BookingDetail entity)
        {
            var tracked = await _context.BookingDetails
            .FirstOrDefaultAsync(bd => bd.BookingReservationId == entity.BookingReservationId && bd.RoomId == entity.RoomId);

            if (tracked != null)
            {
                tracked.StartDate = entity.StartDate;
                tracked.EndDate = entity.EndDate;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(object id)
        {
            if (id is object[] keys && keys.Length == 2)
            {
                int bookingId = (int)keys[0];
                int roomId = (int)keys[1];
                var bookingDetail = await _context.BookingDetails
                    .FirstOrDefaultAsync(bd => bd.BookingReservationId == bookingId && bd.RoomId == roomId);
                if (bookingDetail != null)
                {
                    _context.BookingDetails.Remove(bookingDetail);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<BookingDetail>> GetByBookingIdAsync(int bookingReservationId)
            => await _context.BookingDetails
                .Include(bd => bd.Room)
                .Where(bd => bd.BookingReservationId == bookingReservationId)
                .ToListAsync();

        public Task<BookingDetail> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}