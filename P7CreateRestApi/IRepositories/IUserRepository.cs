//using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Domain;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Threading.Tasks;

namespace P7CreateRestApi.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllUsers();
        Task<IEnumerable<IdentityUser>> GetUserByEmail(string email);
        void CreateUser(IdentityUser user);
        Task UpdateUser(IdentityUser user);
        void DeleteUserByEmail(string email);

    }
}