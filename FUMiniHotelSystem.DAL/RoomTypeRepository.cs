using FUMiniHotelSystem.DAL.Models;

using Microsoft.EntityFrameworkCore;

namespace FUMiniHotelSystem.DAL
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly FUMiniHotelManagementContext _context;

        public RoomTypeRepository(FUMiniHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomType>> GetAllAsync()
            => await _context.RoomTypes.ToListAsync();

        public async Task<RoomType> GetByIdAsync(object id)
            => await _context.RoomTypes.FindAsync(id);

        public async Task AddAsync(RoomType entity)
        {
            await _context.RoomTypes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoomType entity)
        {
            _context.RoomTypes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var type = await _context.RoomTypes.FindAsync(id);
            if (type != null)
            {
                _context.RoomTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RoomType> GetByNameAsync(string name)
            => await _context.RoomTypes.FirstOrDefaultAsync(rt => rt.RoomTypeName == name);
    }
}