using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWebcore6.Data
{
    public class UserRoleService : IUsersRolse
    {
        public const string ADMIN_ROLE_NAME = "ADMIN";
        private const string ADMIN_EMAIL = "sss23@sss.sss";
        private const string ADMIN_PWD = "As12345!";

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRoleService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
                _userManager = userManager;

        }

        private async Task EnsureRoles()
        {
            var existingRole= await _roleManager.FindByNameAsync(ADMIN_ROLE_NAME);
            if (existingRole is null)
            {

                var adminRole = new IdentityRole()
                {
                    Name= ADMIN_ROLE_NAME, NormalizedName=ADMIN_ROLE_NAME.ToUpper()
                };

                await _roleManager.CreateAsync(adminRole);

            }


        }

        private async Task EnsureUsers()
        {
            var existingAdminUser = await _userManager.FindByEmailAsync(ADMIN_EMAIL);

            if (existingAdminUser is null)
            {
                var adminUsers = new IdentityUser()
                {
                    Email = ADMIN_EMAIL,
                    EmailConfirmed = true,
                    UserName = ADMIN_EMAIL,
                    NormalizedEmail = ADMIN_EMAIL.ToUpper(),
                    NormalizedUserName = ADMIN_EMAIL.ToUpper(),
                    LockoutEnabled = false
                };

                await _userManager.CreateAsync(adminUsers, ADMIN_PWD);
            }
        }




    public async Task EnsureAdminUserRole()
        {
            await EnsureRoles();
            await EnsureUsers();

            var existingAdminUser = await _userManager.FindByEmailAsync(ADMIN_EMAIL);
            var ExistingRole = await _roleManager.FindByNameAsync(ADMIN_ROLE_NAME);
            if(existingAdminUser is null || ExistingRole is null)
            {
                throw new InvalidOperationException("cannnot add null user/role combination");
            }

            var userRoles = await _userManager.GetRolesAsync(existingAdminUser);
            var existingUserAdminRole = userRoles.SingleOrDefault(X => X.Equals(ADMIN_ROLE_NAME));

            if(existingUserAdminRole is null)
            {
                await _userManager.AddToRoleAsync(existingAdminUser, ADMIN_ROLE_NAME);

            }

        }
    }
}
