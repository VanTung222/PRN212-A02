using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.DAL;

namespace FUMiniHotelSystem.BLL
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomerService(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
            => await _customerRepo.GetByEmailAsync(email);

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
            => await _customerRepo.GetAllAsync();

        public async Task<Customer> GetCustomerByIdAsync(int id)
            => await _customerRepo.GetByIdAsync(id);

        public async Task AddCustomerAsync(Customer customer)
            => await _customerRepo.AddAsync(customer);

        public async Task UpdateCustomerAsync(Customer customer)
            => await _customerRepo.UpdateAsync(customer);

        public async Task DeleteCustomerAsync(int id)
            => await _customerRepo.DeleteAsync(id);

        public async Task<Customer> AuthenticateAsync(string email, string password)
        {
            var user = await _customerRepo.GetByEmailAsync(email);
            if (user != null && user.Password == password)
                return user;
            return null;
        }

        public async Task<IEnumerable<Customer>> SearchCustomersByNameAsync(string name)
            => await _customerRepo.SearchByNameAsync(name);

        public async Task<IEnumerable<BookingReservation>> GetBookingHistoryAsync(int customerId)
            => await _customerRepo.GetBookingHistoryAsync(customerId);

        public async Task<bool> IsEmailExistsAsync(string email)
            => await _customerRepo.CheckEmailExistsAsync(email);
    }
}