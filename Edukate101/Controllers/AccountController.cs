using Edukate101.Models;
using Edukate101.ViewModel.UserViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Edukate101.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            AppUser user = new()
            {
                UserName = vm.UserName,
                Email = vm.Email
            };

            var result = await userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    return View(vm);
                }

            }

            await userManager.AddToRoleAsync(user, "Member");
            await signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> CreateRoles()
        {
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = "Member"
            });

            return Ok("Ok");

        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVm vm)
        {
            if(!ModelState.IsValid) 
                return View(vm);



            var user = await userManager.FindByEmailAsync(vm.Email);

            if(user == null)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View(vm);
            }

            var result = await signInManager.PasswordSignInAsync(user, vm.Password, false, false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View(vm);
            }
            
            await signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");

        }


    }
}
