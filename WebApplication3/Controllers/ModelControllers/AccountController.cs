using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinanceAnalytic.Controllers
{

    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext dbContextContext)
        {
            _context = dbContextContext;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await   _context.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == model.Password);
                    if (user != null)
                    {
                        var success = await Authenticate(user);
                
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
                catch
                {
                    ModelState.AddModelError("", "Невозможно выполнить вход");
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
              return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVievModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Login == User.Identity.Name && u.Password == model.PasswordOld);          
                if ((user != null))
                {
                    user.Password = model.PasswordNew;     
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("DetailsByLogin", "Users");
                }
                ModelState.AddModelError("", "Текущий пароль введен не верно");
            }
          return View(model);
        }
 
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var viewModel = new RegisterViewModel();
            viewModel.Currencies= new SelectList(await _context.Currencies.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == viewModel.Login);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    UserRole userRole = _context.UsersRoles.FirstOrDefault(r => r.Name== "user");
                    viewModel.Model.UserRole = userRole;                  
                    viewModel.Model.RegistrationDate = DateTime.Now.Date;
                    _context.Users.Add(viewModel.Model);
                    await _context.SaveChangesAsync();

                    await Authenticate(viewModel.Model); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Пользователь с данным логином уже зарегестрирован");
            }
            viewModel.Currencies = new SelectList(await _context.Currencies.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            return View(viewModel);
        }


        private async Task<bool> Authenticate(User user)
        {   
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.Name)     
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity Id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(Id));
            return true;
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
       
    }
}
