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


//using Dot.Net.WebApi.Controllers.Domain;
//using Microsoft.EntityFrameworkCore;
//using P7CreateRestApi.Data;
//using P7CreateRestApi.Domain;
//using P7CreateRestApi.IRepositories;
//using System.Collections;
//using System.Diagnostics;

//namespace P7CreateRestApi.Repositories
//{
//    public class UserRepository : IUserRepository
//    {

//        private static P7Referential? _context;

//        public UserRepository(P7Referential context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<User>> GetAllUsers()
//        {
//            return await _context!.Users.ToListAsync();
//        }

//        public async Task<IEnumerable<User>> GetUserById(int id)
//        {
//            return await _context!.Users.Where(u => u.Id == id)
//                                  .ToListAsync();
//        }

//        public void CreateUser(User user)
//        {
//            if (user != null)
//            {
//                _context!.Users.Add(user);
//                _context.SaveChanges();
//            }
//        }

//        public async Task UpdateUser(User user)
//        {
//            int maxUserId = await GetMaxUserId();
//            if (user != null)
//            {
//                if ((user.Id > 0) && (user.Id <= maxUserId))
//                {
//                    _context.Entry(user).State = EntityState.Modified;
//                    _context!.SaveChanges();
//                }
//            }
//        }


//        public void DeleteUserById(int id)
//        {
//            User user = _context!.Users.First(u => u.Id == id);

//            if (user != null)
//            {
//                _context!.Users.Remove(user);
//                _context!.SaveChanges();
//            }
//        }


//        private static async Task<int> GetMaxUserId()
//        {
//            int maxUserId = _context!.Users.Select(u => u.Id).Max();
//            return await Task.FromResult(maxUserId);
//        }
//    }
//}