using usersManagementAPI.Models;

namespace usersManagementAPI.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(string? name, DateTime? birthdate);
        Task<bool> UpdateUserState(int userId, bool isActive);
        Task<bool> DeleteUser(int userId);
        Task<List<User>> ListAllActiveUsers();
    }
}
