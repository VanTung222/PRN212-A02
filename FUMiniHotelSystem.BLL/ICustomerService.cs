using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.BLL
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer> GetCustomerByIdAsync(int id);

        Task<Customer?> GetCustomerByEmailAsync(string email);

        Task AddCustomerAsync(Customer customer);

        Task UpdateCustomerAsync(Customer customer);

        Task DeleteCustomerAsync(int id);

        Task<Customer> AuthenticateAsync(string email, string password);

        Task<IEnumerable<Customer>> SearchCustomersByNameAsync(string name);

        Task<IEnumerable<BookingReservation>> GetBookingHistoryAsync(int customerId);

        Task<bool> IsEmailExistsAsync(string email);
    }
}