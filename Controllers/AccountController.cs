using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Services;
using ProniaProject.ViewModel;
using System.Data;

namespace ProniaProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ProniaContext _context;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ProniaContext context, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(MemberLoginViewModel loginVM, bool rememberMe = false, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(loginVM.UserName);

            if (user == null || user.IsAdmin)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, rememberMe, false);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your role is incorrect!");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("index", "home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(MemberRegisterViewModel registerVM)
        {
            if (!ModelState.IsValid) return View();

            if (_userManager.Users.Any(x => x.UserName == registerVM.UserName))
            {
                ModelState.AddModelError("UserName", "UserName is alredy taken");
                return View();
            }

            if (_userManager.Users.Any(x => x.Email == registerVM.Email))
            {
                ModelState.AddModelError("Email", "Email is alredy taken");
                return View();
            }

            AppUser user = new AppUser
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err.Description);
                return View();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            await _signInManager.SignInAsync(user, false);

            TempData["Success"] = "Account created successfully";

            return RedirectToAction("index", "home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("login");
            }

            AccountProfileViewModel vm = new AccountProfileViewModel
            {
              Profile = new ProfileEditViewModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                    Address = user.Address,
                    Phone = user.Phone
                },
                Orders = _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Plant).Where(x => x.AppUserId == user.Id).ToList(),
            };
            return View(vm);
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileEditViewModel profileVM)
        {
            if (!ModelState.IsValid)
            {
                AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                return View(vm);
            }

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FullName = profileVM.FullName;
            user.Email = profileVM.Email;
            user.UserName = profileVM.UserName;
            user.Address = profileVM.Address;
            user.Phone = profileVM.Phone;

            if (!string.IsNullOrEmpty(profileVM.CurrentPassword) || !string.IsNullOrEmpty(profileVM.NewPassword) || !string.IsNullOrEmpty(profileVM.ConfirmPassword))
            {


                if (string.IsNullOrEmpty(profileVM.NewPassword) && !string.IsNullOrEmpty(profileVM.CurrentPassword))
                {
                    AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                    ModelState.AddModelError("NewPassword", "Please enter your New Password");
                    return View(vm);

                }
                else if (!string.IsNullOrEmpty(profileVM.NewPassword) && string.IsNullOrEmpty(profileVM.CurrentPassword))
                {
                    AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                    ModelState.AddModelError("CurrentPassword", "Please enter your Current Password");
                    return View(vm);

                }
                else if (profileVM.NewPassword.Length < 8)
                {
                    AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                    ModelState.AddModelError("NewPassword", "New Password length is must be longer than 8");
                    return View(vm);
                }
                else
                {
                    if (!await _userManager.CheckPasswordAsync(user, profileVM.CurrentPassword))
                    {
                        AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                        ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct!!!");
                        return View(vm);
                    }
                    var newPass = await _userManager.ChangePasswordAsync(user, profileVM.CurrentPassword, profileVM.NewPassword);
                    if (!newPass.Succeeded)
                    {
                        foreach (var err in newPass.Errors)
                            ModelState.AddModelError("", err.Description);
                        return View();
                    }
                }

            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                AccountProfileViewModel vm = new AccountProfileViewModel { Profile = profileVM };
                return View(vm);
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("profile");
        }
    }
}
