
using Coursera.Models;

namespace Coursera.Services.SignUp

{
    public interface IUsersService
    {
         Task<User> GetUserByEmailAsync(string email);
        Task AddUserByAsync(User user);
    }
}
