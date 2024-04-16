using Microsoft.EntityFrameworkCore;
using usersManagementAPI.Models;

namespace usersManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly UserManagementDbContext _context;

        public UserService(UserManagementDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(string? name, DateTime? birthdate)
        {
            User user = new User { UserName = name, UserBirthdate = birthdate };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserState(int userId, bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = isActive;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<User>> ListAllActiveUsers()
        {
            return await _context.Users.Where(u => u.IsActive == true).ToListAsync();
        }
    }
}

