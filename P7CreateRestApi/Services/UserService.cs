using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Models;
using P7CreateRestApi.Repositories;
using System.Web.Http.ModelBinding;

namespace P7CreateRestApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _iUserRepository;
        private readonly IMapper _mapper;


        public UserService(IUserRepository iUserRepository, IMapper iMapper)
        {
            _iUserRepository = iUserRepository;
            _mapper = iMapper;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersDto()
        {
            Task<IEnumerable<User>> users = _iUserRepository!.GetAllUsers();
            List<UserDto> ListOfUserDtos = new List<UserDto>();
            foreach (User user in await users)
            {
                UserDto userDto = await MapUserToUserDto(user);
                ListOfUserDtos.Add(userDto);
            }

            return ListOfUserDtos;
        }

        public async Task<IEnumerable<UserDto>> GetUserDtoByEmail(string email)
        {
            Task<IEnumerable<User>> users = _iUserRepository!.GetUserByEmail(email);
            List<UserDto> ListOfUserDtos = new List<UserDto>();
            foreach (User user in await users)
            {
                UserDto userDto = await MapUserToUserDto(user);
                ListOfUserDtos.Add(userDto);
            }

            return ListOfUserDtos;
        }

        public async Task<IdentityResult> CreateUserWithRegisterModel(RegisterModel registerModel)
        {
            //User user = new User() { Email = registerModel.Email, Email = registerModel.Email, Role = registerModel.Role };
            User user = await MapRegisterModelToUser(registerModel);

            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The Mapping failed." });
            }
            IdentityResult result = await _iUserRepository.CreateUser(user);
            return result;

            ////User user = new User() { Email = registerModel.Email, Email = registerModel.Email, Role = registerModel.Role };
            //User user = await MapRegisterModelToToUser(registerModel);
            //if (user != null)
            //{
            //    await _iUserRepository.CreateUser(user);
            //}
        }

        public async Task<IdentityResult> UpdateUserWithUpdateGeneralInfosModel(string email, UpdateGeneralInfosModel updateGeneralInfosModel)
        {
            var userDto = GetUserDtoByEmail(email).Result.Select(u => u).Count();
            bool userExists =  userDto > 0;
            if (!userExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The email does not exist." });
            }
            User user = await MapUpdateGeneralInfosModelToUser(updateGeneralInfosModel); 
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The Mapping failed." });
            }
            IdentityResult result = await _iUserRepository.UpdateUserByEmail(email, user);
            return result;
        }

        public async Task<IdentityResult> UpdateUserPasswordWithUpdatePasswordModel(string email, UpdatePasswordModel updatePasswordModel)
        {            

            var userDtoExists = GetUserDtoByEmail(email).Result.Select(u => u).Count();
            bool userExists = userDtoExists > 0;
            if (!userExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The email does not exist." });
            }
            IEnumerable<UserDto> userDto = await GetUserDtoByEmail(email);
            User user = await MapUserDtoToUser(userDto.FirstOrDefault());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The Mapping failed." });
                
            }
            else
            {
                IdentityResult result = await _iUserRepository.UpdateUserPassword(user, updatePasswordModel);
                return result;
            }

            
            //if (updatePasswordModel != null)
            //{
            //    IEnumerable<UserDto> userDto = await GetUserDtoByEmail(email);
            //    if (userDto != null)
            //    {
            //        User user = await MapUserDtoToUser(userDto.FirstOrDefault());
            //        await _iUserRepository.UpdateUserPassword(user, updatePasswordModel);
            //    }
            //    else
            //    {
            //        return;
            //    }

            //}

        }
        public async Task<IdentityResult> DeleteUserByEmail(string email)
        {
            var userDtoExists = GetUserDtoByEmail(email).Result.Select(u => u).Count();
            bool userExists = userDtoExists > 0;
            if (!userExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The email does not exist." });
            }
            else
            {
                IdentityResult result = await _iUserRepository.DeleteUserByEmail(email);
                return result;
            }
        }

        public async Task<UserDto> MapUserToUserDto(User user)
        {
            UserDto userDto = _mapper.Map<User, UserDto>(user);
            return userDto;
        }
        public async Task<User> MapUserDtoToUser(UserDto userDto)
        {
            User user = _mapper.Map<UserDto, User>(userDto);
            return user;
        }
        public async Task<User> MapRegisterModelToUser(RegisterModel registerModel)
        {
            User user = _mapper.Map<RegisterModel, User>(registerModel);
            return user;
        }
        public async Task<User> MapUpdateGeneralInfosModelToUser(UpdateGeneralInfosModel updateGeneralInfosModel)
        {
            User user = _mapper.Map<UpdateGeneralInfosModel, User>(updateGeneralInfosModel);
            return user;
        }



    }
}
