using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        Task<RoomType> GetByNameAsync(string name);
    }
}