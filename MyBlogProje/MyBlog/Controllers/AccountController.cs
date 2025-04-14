using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class AccountController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (ModelState.IsValid)
            {
                await _signInManager.SignOutAsync();

                var result = await _signInManager.PasswordSignInAsync(user!, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user!);
                    await _userManager.SetLockoutEndDateAsync(user!, null);
                    
                    return RedirectToAction("Index", "Admin");
                }
                else if (result.IsLockedOut)
                {
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user!);
                    var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Hesap Kilitlendi, {timeLeft.Minutes} dakika sonra dene");
                }
                else
                {
                    ModelState.AddModelError("", "Parola Hatalı");

                }
            }
            else
            {
                ModelState.AddModelError("", "Email Hatalı ");
            }
            return View(model);
        }
    }
}