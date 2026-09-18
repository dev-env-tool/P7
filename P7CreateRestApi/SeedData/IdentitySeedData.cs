using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using P7CreateRestApi.Domain;
using System.Threading.Tasks;

namespace P7CreateRestApi.SeedData
{
    public static class IdentitySeedData
    {

        public static async Task EnsurePopulated(WebApplication app)
        {

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Member" };

                foreach (var role in roles)
                {

                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string userAdminEmail = "admin@user.com";
                string userAdminPassword = "Passadmin,123";
                const string role = "Admin";

                if (await userManager.FindByEmailAsync(userAdminEmail) == null)
                {
                    var userAdmin = new User();
                    userAdmin.UserName = userAdminEmail;
                    userAdmin.Email = userAdminEmail;
                    userAdmin.Role = role;
                    userAdmin.Password = userAdminPassword;

                    await userManager.CreateAsync(userAdmin, userAdminPassword);
                    await userManager.AddToRoleAsync(userAdmin, role);
                }

            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string userMemberEmail = "member@user.com";
                string userMemberPassword = "Passmember,123";
                const string role = "Member";

                if (await userManager.FindByEmailAsync(userMemberEmail) == null)
                {
                    var userMember = new User();
                    userMember.UserName = userMemberEmail;
                    userMember.Email = userMemberEmail;
                    userMember.Role = role;
                    userMember.Password = userMemberPassword;

                    await userManager.CreateAsync(userMember, userMemberPassword);
                    await userManager.AddToRoleAsync(userMember, role);
                }
            }
        }
    }
}
