using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index(int? userRole, string Login)
        {
            //Фильтрация
            IQueryable<User> users = _context.Users.Include(p => p.UserRole);
            if (userRole != null && userRole != 0)
            {
                users = users.Where(p => p.UserRoleId == userRole);
            }
            if (!String.IsNullOrEmpty(Login))
            {
                users = users.Where(p => p.Login.Contains(Login));
            }

            List<UserRole> userRoles = _context.UsersRoles.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            userRoles.Insert(0, new UserRole { Name = "Все", Id = 0 });

            UserListViewModel viewModel = new UserListViewModel
            {
                Users = users.ToList(),
                UserRoles = new SelectList(userRoles, "Id", "Name"),
                Login = Login
            };
            return View(viewModel);

            //var appDbContext = _context.Users.Include(u => u.UserRole);
            //return View(await appDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        public async Task<IActionResult> DetailsByLogin()
        {


            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(m => m.Login == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["UserRoleId"] = new SelectList(_context.UsersRoles, "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Password,LastAuthDate,UserRoleId,firstName,lastName")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.AddWithCheckLogin(user);
                   
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ViewData["UserRoleId"] = new SelectList(_context.UsersRoles, "Id", "Name", user.UserRoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["UserRoleId"] = new SelectList(_context.UsersRoles, "Id", "Name", user.UserRoleId);
            return View(user);
        }


  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,LastAuthDate,UserRoleId,firstName,lastName")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                     _context.Update(user);
                   await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            ViewData["UserRoleId"] = new SelectList(_context.UsersRoles, "Id", "Name", user.UserRoleId);
            return View(user);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(user);
            }
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        public ActionResult HasSpendings()
        {
            //  var list = _context.Spendings.Where(c => c.User.Login == User.Identity.Name).GroupBy(g => g.ImportanceCategory.Name).Select(s => new { Name= s.Key.Trim(), cost = s.Sum(i => i.cost), date = s.}).AsQueryable().ToList();
            bool rezult=true;
            int count = _context.Spendings.Where(s => s.User.Login == User.Identity.Name).Count();
            if (count == 0)
            {
                rezult = false;
            }
            return Json(rezult);
        }
    
    }
}
