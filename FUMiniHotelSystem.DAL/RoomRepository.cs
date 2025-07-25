using Microsoft.EntityFrameworkCore;
using FUMiniHotelSystem.DAL.Models;


namespace FUMiniHotelSystem.DAL
{
    public class RoomRepository : IRoomRepository
    {
        private readonly FUMiniHotelManagementContext _context;

        public RoomRepository(FUMiniHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomInformation>> GetAllAsync()
            => await _context.RoomInformations.Include(r => r.RoomType).ToListAsync();

        public async Task<RoomInformation> GetByIdAsync(object id)
            => await _context.RoomInformations.Include(r => r.RoomType).FirstOrDefaultAsync(r => r.RoomId == (int)id);

        public async Task AddAsync(RoomInformation entity)
        {
            await _context.RoomInformations.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoomInformation entity)
        {
            var room = await _context.RoomInformations.FindAsync(entity.RoomId);
            if (room != null)
            {
                room.RoomNumber = entity.RoomNumber;
                room.RoomDetailDescription = entity.RoomDetailDescription;
                room.RoomMaxCapacity = entity.RoomMaxCapacity;
                room.RoomTypeId = entity.RoomTypeId;
                room.RoomStatus = entity.RoomStatus;
                room.RoomPricePerDay = entity.RoomPricePerDay;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(object id)
        {
            var room = await _context.RoomInformations.FindAsync(id);
            if (room != null)
            {
                _context.RoomInformations.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync()
            => await _context.RoomInformations
                .Where(r => r.RoomStatus == 1) 
                .ToListAsync();

        public async Task<IEnumerable<RoomInformation>> SearchRoomAsync(string keyword)
            => await _context.RoomInformations
                .Where(r => r.RoomNumber.Contains(keyword) || r.RoomDetailDescription.Contains(keyword))
                .ToListAsync();

        public async Task<bool> IsRoomDeletableAsync(int roomId)
        {
            var exists = await _context.BookingDetails.AnyAsync(bd => bd.RoomId == roomId);
            return !exists;
        }
    }
}