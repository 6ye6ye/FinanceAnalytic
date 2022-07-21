using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize]
    public class PlanedSpendingsController : Controller
    {
        private readonly AppDbContext _context;

        public PlanedSpendingsController(AppDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index(PlandeSpendingsListViewModel viewModel)
        {
            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }
            ViewBag.SortPeriodBeginParametr = string.IsNullOrEmpty(viewModel.SortParametr) ? "PeriodBegin desc" : "";
            ViewBag.SortPeriodBeginParametr = viewModel.SortParametr == "PeriodEnd" ? "PeriodEnd desc" : "PeriodEnd";
            ViewBag.SortSpendingCategoryParametr = viewModel.SortParametr == "SpendingCategory" ? "SpendingCategory desc" : "SpendingCategory";
            ViewBag.SortSubCategoryParametr = viewModel.SortParametr == "SubCategory" ? "SubCategory desc" : "SubCategory";
            ViewBag.SortSumParametr = viewModel.SortParametr == "Sum" ? "Sum desc" : "Sum";
            var planedSpendingsList = _context.PlanedSpendings.Where(b => b.User.Login == User.Identity.Name).OrderBy(b => b.PeriodBegin);
            switch (viewModel.SortParametr)
            {
                case "Date desc":
                    planedSpendingsList = planedSpendingsList.OrderByDescending(x => x.PeriodBegin);
                    break;
                case "PeriodEnd desc":
                    planedSpendingsList = planedSpendingsList.OrderByDescending(x => x.SpendingCategory.Name);
                    break;
                case "PeriodEnd":
                    planedSpendingsList = planedSpendingsList.OrderBy(x => x.SpendingCategory.Name);
                    break;
                case "SpendingCategory desc":
                    planedSpendingsList = planedSpendingsList.OrderByDescending(x => x.SpendingCategory.Name);
                    break;
                case "SpendingCategory":
                    planedSpendingsList = planedSpendingsList.OrderBy(x => x.SpendingCategory.Name);
                    break;
                case "SubCategory desc":
                    planedSpendingsList = planedSpendingsList.OrderByDescending(x => x.SpendingSubCategory.Name);
                    break;
                case "SubCategory":
                    planedSpendingsList = planedSpendingsList.OrderBy(x => x.SpendingSubCategory.Name);
                    break;
                case "Sum desc":
                    planedSpendingsList = planedSpendingsList.OrderByDescending(x => x.Sum);
                    break;
                case "Sum":
                    planedSpendingsList = planedSpendingsList.OrderBy(x => x.Sum);
                    break;
                default:
                    planedSpendingsList = planedSpendingsList.OrderBy(x => x.PeriodBegin);
                    break;
            }

            viewModel.PlanedSpendings = await planedSpendingsList.ToListAsync();
            var spendingCategory = await _context.SpendingCategories.ToListAsync();
            viewModel.SpendingCategory = new SelectList(spendingCategory, "Id", "Name");
            viewModel.SpendingSubCategory = new SelectList(await _context.SpendingSubCategories.Where(c => c.User.Login == User.Identity.Name && c.SpendingCategoryId == spendingCategory.FirstOrDefault().Id).ToListAsync(), "Id", "Name");
  
            return View(viewModel);
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var PlanedSpendings = await _context.PlanedSpendings
                .Include(b => b.SpendingCategory)
                .Include(b => b.SpendingSubCategory)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (PlanedSpendings == null)
            {
                return NotFound();
            }

            return View(PlanedSpendings);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new PlanedSpendingViewModel { PeriodBegin = DateTime.Now,
                PeriodEnd = DateTime.Now,
            };
            await FillSelectLists(viewModel);
           
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlanedSpendingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                await _context.AddWithCheckCurrency(viewModel.Model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await FillSelectLists(viewModel);
            return View(viewModel);
        }

        // GET: PlanedSpendings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PlanedSpendings = await _context.PlanedSpendings.FindAsync(id);
            if (PlanedSpendings == null)
            {
                return NotFound();
            }
            var PlanedSpendingViewModel = new PlanedSpendingViewModel(PlanedSpendings);
            await FillSelectLists(PlanedSpendingViewModel);
            return View(PlanedSpendingViewModel);
        }

        // POST: PlanedSpendings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlanedSpendingViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).Select(c => c.Id).First();
                    await _context.EditwithCheckCurrency(viewModel.Model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanedSpendingsExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
 
            await FillSelectLists(viewModel);
            return View(viewModel);
        }
        public async Task FillSelectLists(PlanedSpendingViewModel viewModel)
        {
            var SpendingCategory = _context.SpendingCategories;

            SelectList SpendingSubCategories;

            if (viewModel.SpendingSubCategoryId == 0)
            {
                SpendingSubCategories = new SelectList(await _context.SpendingSubCategories
                   .Where(c => c.User.Login == User.Identity.Name && c.SpendingCategoryId == SpendingCategory
                   .FirstOrDefault().Id)
                   .OrderBy(c => c.Id)
                   .ToListAsync(), "Id", "Name", viewModel.SpendingSubCategoryId);
            }
            else
            {
                SpendingSubCategories = new SelectList(await _context.SpendingSubCategories
                 .Where(c => c.User.Login == User.Identity.Name && c.Id == viewModel.SpendingSubCategoryId)
                 .OrderBy(c => c.Id)
                 .ToListAsync(), "Id", "Name", viewModel.SpendingSubCategoryId);
            }
            viewModel.SpendingCategories = new SelectList(await _context.SpendingCategories.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            viewModel.SpendingSubCategories = SpendingSubCategories;
            viewModel.Currencies= new SelectList(await _context.Currencies.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            viewModel.UserCurrencyName = _context.Users.Where(u => u.Login == User.Identity.Name).Select(u => u.Currency.Name).FirstOrDefault();

        }
        // GET: PlanedSpendings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var PlanedSpendings = await _context.PlanedSpendings
                .Include(b => b.SpendingCategory)
                .Include(b => b.SpendingSubCategory)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (PlanedSpendings == null)
            {
                return NotFound();
            }

            return View(PlanedSpendings);
        }

        // POST: PlanedSpendings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var PlanedSpendings = await _context.PlanedSpendings.FindAsync(id);
            _context.PlanedSpendings.Remove(PlanedSpendings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanedSpendingsExists(int id)
        {
            return _context.PlanedSpendings.Any(e => e.Id == id);
        }
    }
}
