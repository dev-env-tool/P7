using P7CreateRestApi.Domain;
using P7CreateRestApi.Models;

namespace P7CreateRestApi.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<IEnumerable<User>> GetUserByEmail(string email);
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task UpdateUserPassword(User user, UpdatePasswordModel updatePasswordModel);
        Task DeleteUserByEmail(string email);

    }
}