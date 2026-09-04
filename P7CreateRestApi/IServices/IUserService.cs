using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.Models;

namespace P7CreateRestApi.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersDto();
        Task<IEnumerable<UserDto>> GetUserDtoByEmail(string email);
        Task CreateUserWithUserDto(UserDto userDto);
        Task UpdateUserWithUpdateGeneralInfosModel(string email, UpdateGeneralInfosModel updateGeneralInfosModel);
        Task UpdateUserPasswordWithUpdatePasswordModel(string email, UpdatePasswordModel updatePasswordModel);
        Task DeleteUserByEmail(string email);
        Task<UserDto> MapUserToUserDto(User user);
        Task<User> MapUserDtoToUser(UserDto userDto);

    }
}