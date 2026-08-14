//using Dot.Net.WebApi.Data;
//using Dot.Net.WebApi.Domain;
//using Microsoft.EntityFrameworkCore;

//namespace Dot.Net.WebApi.Repositories
//{
//    public class UserRepository
//    {

//        private static P7Referential? _context;

//        public UserRepository(P7Referential context)
//        {
//            _context = context;
//        }


//        public User FindByUserName(string userName)
//        {
//            return _context.Users.Where(user => user.UserName == userName)
//                                  .FirstOrDefault();
//        }

//        public async Task<List<User>> FindAllUsers()
//        {
//            return await _context.Users.ToListAsync();
//        }

//        public void Add(User user)
//        {
//        }

//        public User FindById(int id)
//        {
//            return _context.Users.Where(user => user.Id == id)
//                                  .FirstOrDefault();
//        }
//    }
//}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

//using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using System.Collections;
using System.Diagnostics;

namespace P7CreateRestApi.Repositories
{
    public class UserRepository : IUserRepository
    {

        private static ApplicationDbContext? _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            return await _context!.Users.ToListAsync();
        }

        public async Task<IEnumerable<IdentityUser>> GetUserByEmail(string email)
        {
            return await _context!.Users.Where(u => u.UserName == email)
                                  .ToListAsync();
        }

        public void CreateUser(IdentityUser user)
        {
            if (user != null)
            {
                _context!.Users.Add(user);
                _context.SaveChanges();
            }
        }

        public async Task UpdateUser([FromBody] IdentityUser user)
        {
            if (user != null)
            {
                if (GetUserByEmail(user.UserName) != null)
                {
                    _context!.Entry(user).State = EntityState.Modified;
                    _context!.SaveChanges();
                }
            }
        }


        public void DeleteUserByEmail(string email)
        {
            IdentityUser user = _context!.Users.First(u => u.Email == email);

            if (user != null)
            {
                _context!.Users.Remove(user);
                _context!.SaveChanges();
            }
        }

    }
}