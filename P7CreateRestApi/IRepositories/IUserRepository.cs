using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;

namespace P7CreateRestApi.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> GetUserByEmail(string email);
        Task<IdentityResult> CreateUser(User user);
        Task<IdentityResult> UpdateUser(User user);
        Task<IdentityResult> UpdateUserPassword(User user, UpdatePasswordModel updatePasswordModel);
        Task DeleteUserByEmail(string email);

    }
}