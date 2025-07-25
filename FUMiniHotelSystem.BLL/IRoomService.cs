using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.BLL
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomInformation>> GetAllRoomsAsync();

        Task<RoomInformation> GetRoomByIdAsync(int id);

        Task AddRoomAsync(RoomInformation room);

        Task UpdateRoomAsync(RoomInformation room);

        Task DeleteRoomAsync(int id);

        Task<IEnumerable<RoomInformation>> GetAvailableRoomsAsync();

        Task<IEnumerable<RoomInformation>> SearchRoomAsync(string keyword);

        Task<bool> IsRoomDeletableAsync(int roomId);
    }
}