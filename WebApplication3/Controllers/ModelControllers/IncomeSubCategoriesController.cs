using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceAnalytic.Models;
using Microsoft.AspNetCore.Authorization;
using FinanceAnalytic.Services;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize]
    public class IncomeSubCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public IncomeSubCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: IncomeSubCategories
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.IncomeSubCategories.Include(i => i.IncomeCategory).Include(i => i.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: IncomeSubCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IncomeSubCategory = await _context.IncomeSubCategories
                .Include(i => i.IncomeCategory)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (IncomeSubCategory == null)
            {
                return NotFound();
            }

            return View(IncomeSubCategory);
        }

        // GET: IncomeSubCategories/Create
        public IActionResult Create()
        {
            ViewData["IncomeCategoryId"] = new SelectList(_context.IncomeCategories, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login");
            return View();
        }

        // POST: IncomeSubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IncomeCategoryId,UserId")] IncomeSubCategory IncomeSubCategory)
        {
            if (ModelState.IsValid)
            {
                int userId = _context.Users.Where(c => c.Login == User.Identity.Name).Select(c => c.Id).FirstOrDefault();
                IncomeSubCategory.UserId = userId;
                await _context.AddWithCheckNameForUser(IncomeSubCategory);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IncomeCategoryId"] = new SelectList(_context.IncomeCategories, "Id", "Name", IncomeSubCategory.IncomeCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", IncomeSubCategory.UserId);
            return View(IncomeSubCategory);
        }

        // GET: IncomeSubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IncomeSubCategory = await _context.IncomeSubCategories.FindAsync(id);
            if (IncomeSubCategory == null)
            {
                return NotFound();
            }
            ViewData["IncomeCategoryId"] = new SelectList(_context.IncomeCategories, "Id", "Name", IncomeSubCategory.IncomeCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", IncomeSubCategory.UserId);
            return View(IncomeSubCategory);
        }

        // POST: IncomeSubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IncomeCategoryId,UserId")] IncomeSubCategory IncomeSubCategory)
        {
            if (id != IncomeSubCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.EditwithCheckNameForUser(IncomeSubCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            ViewData["IncomeCategoryId"] = new SelectList(_context.IncomeCategories, "Id", "Name", IncomeSubCategory.IncomeCategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", IncomeSubCategory.UserId);
            return View(IncomeSubCategory);
        }

        // GET: IncomeSubCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IncomeSubCategory = await _context.IncomeSubCategories
                .Include(i => i.IncomeCategory)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (IncomeSubCategory == null)
            {
                return NotFound();
            }

            return View(IncomeSubCategory);
        }

        // POST: IncomeSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var IncomeSubCategory = await _context.IncomeSubCategories.FindAsync(id);
            _context.IncomeSubCategories.Remove(IncomeSubCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomeSubCategoryExists(int id)
        {
            return _context.IncomeSubCategories.Any(e => e.Id == id);
        }

        public JsonResult GetIncomeSubCategories(int id)
        {
            var list = _context.IncomeSubCategories.Where(c => c.IncomeCategoryId == id && c.User.Login == User.Identity.Name).ToList();
            var a = new SelectList(list, "Id", "Name");
            return Json(a);
        }
    }
}
