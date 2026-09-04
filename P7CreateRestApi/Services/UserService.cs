using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Models;


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

        public async Task CreateUserWithUserDto(UserDto userDto)
        {
            User user = await MapUserDtoToUser(userDto);
            if (userDto != null)
            {
                await _iUserRepository.CreateUser(user);
            }
        }

        public async Task UpdateUserWithUpdateGeneralInfosModel(string email, UpdateGeneralInfosModel updateGeneralInfosModel)
        {
            if (updateGeneralInfosModel != null)
            {
                User user = new User()
                {
                    Email = email,
                    UserName = updateGeneralInfosModel.UserName,
                    Role = updateGeneralInfosModel.Role,
                };
                await _iUserRepository.UpdateUser(user);
            }
        }

        public async Task UpdateUserPasswordWithUpdatePasswordModel(string email, UpdatePasswordModel updatePasswordModel)
        {
            if (updatePasswordModel != null)
            {
                IEnumerable<UserDto> userDto = await GetUserDtoByEmail(email);
                if (userDto != null)
                {
                    User user = await MapUserDtoToUser(userDto.FirstOrDefault());
                    await _iUserRepository.UpdateUserPassword(user, updatePasswordModel);
                }
                else
                {
                    return;
                }

            }

        }
        public async Task DeleteUserByEmail(string email)
        {
            await _iUserRepository.DeleteUserByEmail(email);
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



    }
}
