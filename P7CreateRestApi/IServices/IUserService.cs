using Microsoft.AspNetCore.Identity;
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
        Task<IdentityResult> CreateUserWithRegisterModel(RegisterModel registerModel);
        Task<IdentityResult> UpdateUserWithUpdateGeneralInfosModel(string email, UpdateGeneralInfosModel updateGeneralInfosModel);
        Task<IdentityResult> UpdateUserPasswordWithUpdatePasswordModel(string email, UpdatePasswordModel updatePasswordModel);
        Task<IdentityResult> DeleteUserByEmail(string email);
        Task<UserDto> MapUserToUserDto(User user);
        Task<User> MapUserDtoToUser(UserDto userDto);
        Task<User> MapRegisterModelToUser(RegisterModel registerModel);
        Task<User> MapUpdateGeneralInfosModelToUser(UpdateGeneralInfosModel updateGeneralInfosModel);

    }
}