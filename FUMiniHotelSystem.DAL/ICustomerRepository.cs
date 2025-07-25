using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);

        Task<IEnumerable<Customer>> SearchByNameAsync(string name);

        Task<IEnumerable<BookingReservation>> GetBookingHistoryAsync(int customerId);

        Task<bool> CheckEmailExistsAsync(string email);
    }
}