using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public interface IRoomRepository : IRepository<RoomInformation>
    {
        Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync();

        Task<IEnumerable<RoomInformation>> SearchRoomAsync(string keyword);

        Task<bool> IsRoomDeletableAsync(int roomId); // Kiểm tra có thể xóa không
    }
}