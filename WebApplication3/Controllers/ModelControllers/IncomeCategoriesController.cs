using FinanceAnalytic.Models;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize(Roles = "admin")]
    public class IncomeCategorysController : Controller
    {
        private readonly AppDbContext _context;

        public IncomeCategorysController(AppDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _context.IncomeCategories.ToListAsync());
        }

        // GET: IncomeCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var IncomeCategory = await _context.IncomeCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (IncomeCategory == null)
            {
                return NotFound();
            }

            return View(IncomeCategory);
        }

        // GET: IncomeCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] IncomeCategory IncomeCategory)
        {
            if (ModelState.IsValid)
            {
                await _context.AddWithCheckName(IncomeCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(IncomeCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var IncomeCategory = await _context.IncomeCategories.FindAsync(id);
            if (IncomeCategory == null)
            {
                return NotFound();
            }
            return View(IncomeCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] IncomeCategory IncomeCategory)
        {
            if (id != IncomeCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.EditwithCheckName(IncomeCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(IncomeCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var IncomeCategory = await _context.IncomeCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (IncomeCategory == null)
            {
                return NotFound();
            }

            return View(IncomeCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var IncomeCategory = await _context.IncomeCategories.FindAsync(id);
            _context.IncomeCategories.Remove(IncomeCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomeCategoryExists(int id)
        {
            return _context.IncomeCategories.Any(e => e.Id == id);
        }
    }
}
