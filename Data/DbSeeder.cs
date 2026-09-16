using Microsoft.AspNetCore.Identity;
using SC.Constants;

//populate a database with initial data during deployment or development
namespace SC.Data
{
    public class DbSeeder
    {
        // interface that acts as the foundational runtime engine for resolving object instances from a Dependency Injection (DI) container
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();

            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123"); //Create user & password
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }

        }
    }
}
