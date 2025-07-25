using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.BLL
{
    public interface IRoomTypeService
    {
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync();

        Task<RoomType> GetRoomTypeByIdAsync(int id);

        Task AddRoomTypeAsync(RoomType type);

        Task UpdateRoomTypeAsync(RoomType type);

        Task DeleteRoomTypeAsync(int id);

        Task<RoomType> GetRoomTypeByNameAsync(string name);
    }
}