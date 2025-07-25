using Microsoft.EntityFrameworkCore;
using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelSystem.DAL
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly FUMiniHotelManagementContext _context;

        public CustomerRepository(FUMiniHotelManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync() =>
            await _context.Customers.ToListAsync();

        public async Task<Customer> GetByIdAsync(object id) =>
            await _context.Customers.FindAsync(id);

        public async Task AddAsync(Customer entity)
        {
            await _context.Customers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer entity)
        {
            var existing = await _context.Customers.FindAsync(entity.CustomerId);
            if (existing != null)
            {
                existing.CustomerFullName = entity.CustomerFullName;
                existing.Telephone = entity.Telephone;
                existing.EmailAddress = entity.EmailAddress;
                existing.CustomerBirthday = entity.CustomerBirthday;
                existing.CustomerStatus = entity.CustomerStatus;

                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(object id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Customer> GetByEmailAsync(string email) =>
            await _context.Customers.FirstOrDefaultAsync(c => c.EmailAddress == email);

        public async Task<IEnumerable<Customer>> SearchByNameAsync(string name) =>
            await _context.Customers.Where(c => c.CustomerFullName.Contains(name)).ToListAsync();

        public async Task<IEnumerable<BookingReservation>> GetBookingHistoryAsync(int customerId) =>
            await _context.BookingReservations
                .Where(br => br.CustomerId == customerId)
                .OrderByDescending(br => br.BookingDate)
                .ToListAsync();

        public async Task<bool> CheckEmailExistsAsync(string email) =>
            await _context.Customers.AnyAsync(c => c.EmailAddress == email);
    }
}