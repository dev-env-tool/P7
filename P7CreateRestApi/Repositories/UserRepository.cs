using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
            return await _context!.Users.Where(u => u.Email == email)
                                  .ToListAsync();
        }

        public async Task<IdentityResult> CreateUser(User user)
        {

            var result = await _userManager.CreateAsync(user, user.Password);
            bool roleIsValid1 = user.Role.Equals("Member");
            bool roleIsValid2 = user.Role.Equals("Admin");

            if (user != null && result.Succeeded)
            {

                if (user.Role == "" || user.Role == null)
                {
                    user.Role = "Member";
                    await _userManager.AddToRoleAsync(user, user.Role);
                    return result;
                }

                if ((!roleIsValid1) && (!roleIsValid2))
                {
                    user.Role = "Member";
                    await _userManager.AddToRoleAsync(user, user.Role);
                    return result;
                }
                if ((roleIsValid1) || (roleIsValid2))
                {
                    var test = await _userManager.AddToRoleAsync(user, user.Role);
                    int test2 = 1;
                    return result;
                }

            }
            return result;
        }

        public async Task<IdentityResult> UpdateUserByEmail(string email, User user)
        {
            //User userToFind = GetUserByEmail(email).Result.First();
            var userToFind = await _userManager.FindByEmailAsync(email);
            if (userToFind == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User does not exist." });
            }
            else
            {
                if (user.UserName != null)
                {
                    
                    var setUserNameAsync = await _userManager.SetUserNameAsync(userToFind, user.UserName);
                    if (!setUserNameAsync.Succeeded)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "The new username could not be changed." });
                    }
                    
                    Task setNormalizedUserName = _userManager.UpdateNormalizedUserNameAsync(userToFind);
                    if (!setNormalizedUserName.IsCompletedSuccessfully || setNormalizedUserName.IsCanceled || setNormalizedUserName.IsFaulted)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "The new normalizedusername could not be changed." });
                    }
                    await _userManager.UpdateAsync(userToFind);
                }
                if (user.Role != null)
                {
                    var roleUpdateResult = await _userManager.AddToRoleAsync(userToFind, user.Role);
                    if (!roleUpdateResult.Succeeded)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "The new role could not be added to the user. The user is already in role " + user.Role });
                    }


                    var roleRemoveResult = await _userManager.RemoveFromRoleAsync(userToFind, userToFind.Role);
                    if (!roleRemoveResult.Succeeded)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "The previous role could not be removed from the user." });
                    }

                    userToFind.Role = user.Role;
                    var updateRoleinUser = await _userManager.UpdateAsync(userToFind);
                    if (!updateRoleinUser.Succeeded)
                    {
                        return IdentityResult.Failed(new IdentityError { Description = "The new front-end role could not be added to the user." });
                    }
                }

                string test = userToFind.UserName;
                string test2 = userToFind.NormalizedUserName;

                return IdentityResult.Success;

            }
            //    bool IsInRole = await _userManager.IsInRoleAsync(userToFind, user.Role);
            //    if (!IsInRole)
            //    {
            //        IdentityResult roleUpdateResult = await _userManager.AddToRoleAsync(userToFind, user.Role);
            //        IEnumerable<IdentityError> errors = roleUpdateResult.Errors;
            //        IList<string> foundRoles = await _userManager.GetRolesAsync(userToFind);
            //        foundRoles.Remove(user.Role);
            //        foreach (var role in foundRoles)
            //        {
            //            await _userManager.RemoveFromRoleAsync(userToFind, role);
            //        }
            //        return roleUpdateResult;
            //    }
            //    userToFind.Email = user.Email;
            //    userToFind.Role = user.Role;
            //    var resultFoundUser = await _userManager.UpdateAsync(userToFind);
            //    return resultFoundUser;
            //}
            //return IdentityResult.Failed(new IdentityError { Description = "The update failed." });


            //if (user != null)
            //{
            //    if (GetUserByEmail(user.Email) != null)
            //    {
            //        _context!.Entry(user).State = EntityState.Modified;
            //        _context!.SaveChanges();
            //    }
            //}

        }

        public async Task<IdentityResult> UpdateUserPassword(User user, UpdatePasswordModel updatePasswordModel)
        {
            User userToFind = await _userManager.FindByEmailAsync(user.Email);
            if (userToFind == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User does not exist." });
            }
            else
            {
                var result = await _userManager.ChangePasswordAsync(userToFind, updatePasswordModel.CurrentPassword, updatePasswordModel.NewPassword);
                if (result.Succeeded)
                {
                    userToFind.Password = updatePasswordModel.NewPassword;
                    await _userManager.UpdateAsync(userToFind);
                }
                return result;
            }
            //User userToFind = await _userManager.FindByEmailAsync(user.Email);
            //if (userToFind == null)
            //{
            //    return;
            //}
            //else
            //{
            //    var result = await _userManager.ChangePasswordAsync(userToFind, updatePasswordModel.CurrentPassword, updatePasswordModel.NewPassword);
            //}

        }


        public async Task<IdentityResult> DeleteUserByEmail(string email)
        {
            User userToFind = await _userManager.FindByEmailAsync(email);
            if (userToFind == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User does not exist." });
            }
            else
            {
                var result = await _userManager.DeleteAsync(userToFind);
                return result;
            }

        }

    }
}