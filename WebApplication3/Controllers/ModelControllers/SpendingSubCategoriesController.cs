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
    [Authorize]
    public class SpendingSubCategoriesController : Controller
    {
        private readonly AppDbContext _context;
   
        public SpendingSubCategoriesController(AppDbContext context)
        {
            _context = context;
        }


         public async Task<IActionResult> Index(int SpendingCategory, string sortBy)
        {
            ViewBag.SortDateParametr = string.IsNullOrEmpty(sortBy) ? "Namedesc" : "";
            ViewBag.SortSpendingCategoryParametr = sortBy == "SpendingCategory" ? "SpendingCategory desc" : "SpendingCategory";
            var SpendingSubCategory = _context.SpendingSubCategories.Include(p => p.SpendingCategory).Where(b => b.User.Login == User.Identity.Name).OrderBy(b => b.Name).AsQueryable();

            switch (sortBy)
            {
                case "Namedesc":
                    SpendingSubCategory = SpendingSubCategory.OrderByDescending(x => x.Name);
                    break;
                case "SpendingCategory desc":
                    SpendingSubCategory = SpendingSubCategory.OrderByDescending(x => x.SpendingCategory.Name);
                    break;
                case "SpendingCategory":
                    SpendingSubCategory = SpendingSubCategory.OrderBy(x => x.SpendingCategory.Name);
                    break;
              
                default:
                    SpendingSubCategory = SpendingSubCategory.OrderBy(x => x.Name);
                    break;

            }
           if (SpendingCategory != 0)
            {
                SpendingSubCategory = SpendingSubCategory.Where(p => p.SpendingCategoryId == SpendingCategory);
            }
         
         
            List<SpendingCategory> spendingCategories = _context.SpendingCategories.OrderBy(c => c.Name).ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            spendingCategories.Insert(0, new SpendingCategory { Name= "Все", Id = 0 });

            SpendingSubCategorieListView viewModel = new SpendingSubCategorieListView
            {

                SpendingSubCategory = SpendingSubCategory.ToList(),
                SpendingCategories = new SelectList(spendingCategories, "Id", "Name")
              
            };
            return View( viewModel);
            //var appDbContext = _context.SubCategory.Include(s => s.SpendingCategory).Include(s => s.User.Login);
            //return View(await appDbContext.ToListAsync());
        }

        // GET: SpendingSubCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SpendingSubCategory = await _context.SpendingSubCategories
                .Include(s => s.SpendingCategory)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SpendingSubCategory == null)
            {
                return NotFound();
            }

            return View(SpendingSubCategory);
        }

        public IActionResult Create()
        {
            ViewData["SpendingCategoryId"] = new SelectList(_context.SpendingCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,SpendingCategoryId,UserId")] SpendingSubCategory SpendingSubCategory)
        {
            if (ModelState.IsValid)
            {
                SpendingSubCategory.User = _context.Users.Where(c => c.Login == User.Identity.Name).First();
                try { 
                    await _context.AddWithCheckNameForUser(SpendingSubCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            ViewData["SpendingCategoryId"] = new SelectList(_context.SpendingCategories, "Id", "Name", SpendingSubCategory.SpendingCategoryId);
            return View(SpendingSubCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SpendingSubCategory = await _context.SpendingSubCategories.FindAsync(id);
            if (SpendingSubCategory == null)
            {
                return NotFound();
            }
            ViewData["SpendingCategoryId"] = new SelectList(_context.SpendingCategories, "Id", "Name", SpendingSubCategory.SpendingCategoryId);       
            return View(SpendingSubCategory);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SpendingCategoryId,UserId")] SpendingSubCategory SpendingSubCategory)
        {
            if (id != SpendingSubCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = _context.Users.Where(c => c.Login == User.Identity.Name).FirstOrDefault();
                SpendingSubCategory.UserId = user.Id;
                try
                {
                    await _context.EditwithCheckNameForUser(SpendingSubCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpendingSubCategorieExists(SpendingSubCategory.Id))
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
           ViewData["SpendingCategoryId"] = new SelectList(_context.SpendingCategories, "Id", "Name", SpendingSubCategory.SpendingCategoryId);
            return View(SpendingSubCategory);
        }

        // GET: SpendingSubCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var SpendingSubCategory = await _context.SpendingSubCategories
                .Include(s => s.SpendingCategory)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (SpendingSubCategory == null)
            {
                return NotFound();
            }

            return View(SpendingSubCategory);
        }

        // POST: SpendingSubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var SpendingSubCategory = await _context.SpendingSubCategories.FindAsync(id);
            try
            {
                await _context.SpendingSubCategoriesDelete(SpendingSubCategory);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex) {
                ModelState.AddModelError("", ex.Message);
                return View(SpendingSubCategory);
            }
        }

        private bool SpendingSubCategorieExists(int id)
        {
            return _context.SpendingSubCategories.Any(e => e.Id == id);
        }

        public JsonResult GetSpendingSubCategories(int id)
        {
            var list = _context.SpendingSubCategories.Where(c => c.SpendingCategoryId == id && c.User.Login == User.Identity.Name).ToList();
            var a = new SelectList(list, "Id", "Name");
            return Json(a);
        }
    }
}
