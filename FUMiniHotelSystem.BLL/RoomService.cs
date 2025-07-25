using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.DAL;

namespace FUMiniHotelSystem.BLL
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepo;

        public RoomService(IRoomRepository roomRepo)
        {
            _roomRepo = roomRepo;
        }

        public async Task<IEnumerable<RoomInformation>> GetAllRoomsAsync()
            => await _roomRepo.GetAllAsync();

        public async Task<RoomInformation> GetRoomByIdAsync(int id)
            => await _roomRepo.GetByIdAsync(id);

        public async Task AddRoomAsync(RoomInformation room)
            => await _roomRepo.AddAsync(room);

        public async Task UpdateRoomAsync(RoomInformation room)
            => await _roomRepo.UpdateAsync(room);

        public async Task DeleteRoomAsync(int id)
            => await _roomRepo.DeleteAsync(id);

        public async Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync()
            => await _roomRepo.GetAvailableRoomsAsync();

        public async Task<IEnumerable<RoomInformation>> SearchRoomAsync(string keyword)
            => await _roomRepo.SearchRoomAsync(keyword);

        public async Task<bool> IsRoomDeletableAsync(int roomId)
            => await _roomRepo.IsRoomDeletableAsync(roomId);
    }
}