using Kalakobana.Auth.Domain;
using Kalakobana.Auth.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace Kalakobana.Auth.Infrastructure.Services
{
    public class IdentityInitializer
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public IdentityInitializer(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedCredentialsAsync()
        {
            string[] roles = { Roles.Admin.ToString(), Roles.User.ToString() };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new Role() { Name = role });
            }

            //On prod admin credentials will be stored in Azure KeyVault!
            var adminEmail = "gchikovanii25@kalakobana.ge";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser is null)
            {
                var user = new User
                {
                    UserName = "GCHIK25ADMIN",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "TestPassword1");

                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

    }
}
