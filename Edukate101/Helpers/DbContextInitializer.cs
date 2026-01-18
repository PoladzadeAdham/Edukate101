using Edukate101.Models;
using Edukate101.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Identity;

namespace Edukate101.Helpers
{
    public class DbContextInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AdminVm _adminVm;

        public DbContextInitializer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _adminVm = _configuration.GetSection("AdminSetting").Get<AdminVm>() ?? new();
        }

        public async Task InitializeDatabase()
        {
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Admin"
            });

            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "Member"
            });

            AppUser user = new()
            {
                UserName = _adminVm.Username,
                Email = _adminVm.Email,
            };

            var result = await _userManager.CreateAsync(user, _adminVm.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user,"Admin");
            }

        }

    }
}
