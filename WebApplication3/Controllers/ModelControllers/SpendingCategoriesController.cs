using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceAnalytic.Models;
using FinanceAnalytic.Services;
using System;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize(Roles = "admin")]
    public class SpendingCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public SpendingCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: SpendingCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.SpendingCategories.OrderBy(c=>c.Name).ToListAsync());
        }

        // GET: SpendingCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SpendingCategory = await _context.SpendingCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SpendingCategory == null)
            {
                return NotFound();
            }

            return View(SpendingCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SpendingCategory SpendingCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.AddWithCheckName(SpendingCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            
            }
            return View(SpendingCategory);
        }

        // GET: SpendingCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SpendingCategory = await _context.SpendingCategories.FindAsync(id);
            if (SpendingCategory == null)
            {
                return NotFound();
            }
            return View(SpendingCategory);
        }

        // POST: SpendingCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SpendingCategory SpendingCategory)
        {
            if (id != SpendingCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.EditwithCheckName(SpendingCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(SpendingCategory);
        }

        // GET: SpendingCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SpendingCategory = await _context.SpendingCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SpendingCategory == null)
            {
                return NotFound();
            }

            return View(SpendingCategory);
        }

        // POST: SpendingCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var SpendingCategory = await _context.SpendingCategories.FindAsync(id);
            try
            {
                
                _context.SpendingCategories.Remove(SpendingCategory);
#pragma warning disable CS4014 // Поскольку этот вызов не ожидается, выполнение текущего метода продолжается до завершения вызова. Попробуйте применить оператор await к результату вызова.
                _context.SaveChangesAsync();
#pragma warning restore CS4014 // Поскольку этот вызов не ожидается, выполнение текущего метода продолжается до завершения вызова. Попробуйте применить оператор await к результату вызова.
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(SpendingCategory); 
            }
            
        }

        private bool SpendingCategorieExists(int id)
        {
            return _context.SpendingCategories.Any(e => e.Id == id);
        }
    }
}
