
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinanceAnalytic.Models;
using System;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize(Roles = "admin")]
    public class ImportanceCategorysController : Controller
    {
        private readonly AppDbContext _context;

        public ImportanceCategorysController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ImportanceCategories.OrderBy(c=>c.NumericEquivalent).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ImportanceCategory = await _context.ImportanceCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ImportanceCategory == null)
            {
                return NotFound();
            }


            return View(ImportanceCategory);
        }

        // GET: ImportanceCategoriesId/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImportanceCategoriesId/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,NumericEquivalent")] ImportanceCategory ImportanceCategory)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.AddWithCheckName(ImportanceCategory);
                    return RedirectToAction(nameof(Index));
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(ImportanceCategory);
        }

        // GET: ImportanceCategoriesId/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ImportanceCategory = await _context.ImportanceCategories.FindAsync(id);
            if (ImportanceCategory == null)
            {
                return NotFound();
            }
            return View(ImportanceCategory);
        }

        // POST: ImportanceCategoriesId/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,NumericEquivalent")] ImportanceCategory ImportanceCategory)
        {
            if (id != ImportanceCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {
                    await _context.EditwithCheckName(ImportanceCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
              
              
            }
            return View(ImportanceCategory);
        }

        // GET: ImportanceCategoriesId/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ImportanceCategory = await _context.ImportanceCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ImportanceCategory == null)
            {
                return NotFound();
            }

            return View(ImportanceCategory);
        }

        // POST: ImportanceCategoriesId/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ImportanceCategory = await _context.ImportanceCategories.FindAsync(id);
            try
            {
                _context.ImportanceCategories.Remove(ImportanceCategory);
                return RedirectToAction(nameof(Index));
            }
            catch {
                ModelState.AddModelError("", "Ошибка. Удаление отменено.");
                return View(ImportanceCategory);
            }
           
     
        }

        private bool ImportanceCategoryExists(int id)
        {
            return _context.ImportanceCategories.Any(e => e.Id == id);
        }
    }
}
