using Coursera.Data;
using Coursera.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursera.Services.SignUp
{
    public class UserService:IUsersService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context) 
        { 
            this._context = context;    
        }

        public async Task AddUserByAsync(User user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.users.Include(u => u.Role).FirstOrDefaultAsync(u=>u.Email==email);
        }
    }
}
