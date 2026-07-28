using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Repositories
{
    public class UserRepository
    {

        private static P7Referential? _context;

        public UserRepository(P7Referential context)
        {
            _context = context;
        }


        public User FindByUserName(string userName)
        {
            return _context.Users.Where(user => user.UserName == userName)
                                  .FirstOrDefault();
        }

        public async Task<List<User>> FindAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public void Add(User user)
        {
        }

        public User FindById(int id)
        {
            return _context.Users.Where(user => user.Id == id)
                                  .FirstOrDefault();
        }
    }
}