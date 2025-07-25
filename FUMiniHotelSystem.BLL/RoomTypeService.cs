using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.DAL;

namespace FUMiniHotelSystem.BLL
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepo;

        public RoomTypeService(IRoomTypeRepository roomTypeRepo)
        {
            _roomTypeRepo = roomTypeRepo;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync()
            => await _roomTypeRepo.GetAllAsync();

        public async Task<RoomType> GetRoomTypeByIdAsync(int id)
            => await _roomTypeRepo.GetByIdAsync(id);

        public async Task AddRoomTypeAsync(RoomType type)
            => await _roomTypeRepo.AddAsync(type);

        public async Task UpdateRoomTypeAsync(RoomType type)
            => await _roomTypeRepo.UpdateAsync(type);

        public async Task DeleteRoomTypeAsync(int id)
            => await _roomTypeRepo.DeleteAsync(id);

        public async Task<RoomType> GetRoomTypeByNameAsync(string name)
            => await _roomTypeRepo.GetByNameAsync(name);
    }
}