using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.Models;
using static Duende.IdentityServer.Models.IdentityResources;

namespace P7CreateRestApi.Repositories
{
    public class UserRepository : IUserRepository
    {

        private static ApplicationDbContext? _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext context, 
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context!.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUserByEmail(string email)
        {
            return await _context!.Users.Where(u => u.UserName == email)
                                  .ToListAsync();
        }

        public async Task CreateUser(User user)
        {
            if (user != null)
            {
                await _userManager.CreateAsync(user, user.Password);
                await _userManager.AddToRoleAsync(user, user.Role);
            }

        }

        public async Task UpdateUser(User user)
        {
            User userToFind = await _userManager.FindByEmailAsync(user.Email);
            if (userToFind == null)
            {
                return;
            }
            else
            {
                userToFind.UserName = user.UserName;
                var result = await _userManager.UpdateAsync(userToFind);

                if(user.Role != null) 
                {
                    if (!await _userManager.IsInRoleAsync(userToFind, user.Role))
                    {
                        if (!await _roleManager.RoleExistsAsync(user.Role))
                        {
                            return;
                        }
                        else
                        {
                            var roleUpdateResult = await _userManager.AddToRoleAsync(userToFind, user.Role);
                            IList<string> foundRoles = await _userManager.GetRolesAsync(userToFind);
                            foundRoles.Remove(user.Role);
                            foreach (var role in foundRoles)
                            {
                                await _userManager.RemoveFromRoleAsync(userToFind, role);
                            }
                        }
                    }
                }

            }
            //if (user != null)
            //{
            //    if (GetUserByEmail(user.UserName) != null)
            //    {
            //        _context!.Entry(user).State = EntityState.Modified;
            //        _context!.SaveChanges();
            //    }
            //}
        }


        public async Task UpdateUserPassword(User user, UpdatePasswordModel updatePasswordModel)
        {
            User userToFind = await _userManager.FindByEmailAsync(user.Email);
            if (userToFind == null)
            {
                return;
            }
            else
            {
                var result = await _userManager.ChangePasswordAsync(userToFind, updatePasswordModel.CurrentPassword, updatePasswordModel.NewPassword);
            }

        }


        public async Task DeleteUserByEmail(string email)
        {
            User user = _context!.Users.First(u => u.Email == email);

            if (user != null)
            {
                _context!.Users.Remove(user);
                _context!.SaveChanges();
            }

        }

    }
}