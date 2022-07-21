using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize]
    public class SpendingsController : Controller
    {
        private readonly AppDbContext _context;


        public SpendingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Spendings
        public async Task<IActionResult> Index(SpendingsListViewModel viewModel)
        {
            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }
            ViewBag.SortDateParametr = string.IsNullOrEmpty(viewModel.SortParametr) ? "Date desc" : "";
            ViewBag.SortSpendingCategoryParametr = viewModel.SortParametr == "SpendingCategory" ? "SpendingCategory desc" : "SpendingCategory";
            ViewBag.SortSubCategoryParametr = viewModel.SortParametr == "SubCategory" ? "SubCategory desc" : "SubCategory";
            ViewBag.SortImportanceCategoryParametr = viewModel.SortParametr == "ImportanceCategory" ? "ImportanceCategory desc" : "ImportanceCategory";
            ViewBag.SortSumParametr = viewModel.SortParametr == "Sum" ? "Cost desc" : "Sum";


            var spendingsList = AnalyticHelper.GetSpendingsQueryableWithFilters(User.Identity.Name, viewModel, _context);
            switch (viewModel.SortParametr)
            {
                case "Date desc":
                    spendingsList = spendingsList.OrderByDescending(x => x.Date);
                    break;
                case "SpendingCategory desc":
                    spendingsList = spendingsList.OrderByDescending(x => x.SpendingCategory.Name);
                    break;
                case "SpendingCategory":
                    spendingsList = spendingsList.OrderBy(x => x.SpendingCategory.Name);
                    break;
                case "SubCategory desc":
                    spendingsList = spendingsList.OrderByDescending(x => x.SpendingSubCategory.Name);
                    break;
                case "SubCategory":
                    spendingsList = spendingsList.OrderBy(x => x.SpendingSubCategory.Name);
                    break;
                case "ImportanceCategory desc":
                    spendingsList = spendingsList.OrderByDescending(x => x.ImportanceCategory.NumericEquivalent);
                    break;
                case "ImportanceCategory":
                    spendingsList = spendingsList.OrderBy(x => x.ImportanceCategory.NumericEquivalent);
                    break;
                case "Cost desc":
                    spendingsList = spendingsList.OrderByDescending(x => x.Sum);
                    break;
                case "Sum":
                    spendingsList = spendingsList.OrderBy(x => x.Sum);
                    break;
                default:
                    spendingsList = spendingsList.OrderBy(x => x.Date);
                    break;
            }
            var spendingCategories = _context.SpendingCategories
                .OrderBy(c => c.Name)
                .ToList();
            var spendingSubCategories = _context.SpendingSubCategories
             .OrderBy(c => c.Name)
             .Where(c => c.SpendingCategoryId == viewModel.SelectedSpendingCategory && c.User.Login == User.Identity.Name)
             .ToList();

            var importanceCategoriesId = _context.ImportanceCategories
                .OrderBy(c => c.Name)
                .ToList();


            viewModel.Spendings = spendingsList.ToList();
            viewModel.SpendingCategory = new SelectList(spendingCategories, "Id", "Name");
            viewModel.SpendingSubCategory = new SelectList(spendingSubCategories, "Id", "Name");
            viewModel.ImportanceCategory = new SelectList(importanceCategoriesId, "Id", "Name");
            return View(viewModel);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spending = await _context.Spendings
                .Include(s => s.ImportanceCategory)
                .Include(s => s.SpendingCategory)
                .Include(s => s.SpendingSubCategory)
                .Include(s => s.Currency)
                //   .Include(s => s.Unit)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spending == null)
            {
                return NotFound();
            }

            return View(spending);
        }

        // GET: Spendings/Create
        public async Task<IActionResult> Create()
        {
            var SpendingsViewModel = new SpendingsViewModel { Date = DateTime.Now };
            await FillSelectListsAsync(SpendingsViewModel);
            SpendingsViewModel.Date = DateTime.Now;
            SpendingsViewModel.UserCurrencyName = _context.Users.Where(u => u.Login == User.Identity.Name).Select(u => u.Currency.Name).FirstOrDefault();
            return View(SpendingsViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpendingsViewModel SpendigsListView)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    SpendigsListView.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                    await _context.AddWithCheckCurrency(SpendigsListView.Model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            await FillSelectListsAsync(SpendigsListView);
            return View(SpendigsListView);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spending = await _context.Spendings.FindAsync(id);
            if (spending == null)
            {
                return NotFound();
            }
            var SpendingsViewModel = new SpendingsViewModel(spending);
            await FillSelectListsAsync(SpendingsViewModel);

            return View(SpendingsViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,SpendingCategoryId,SpendingSubCategoryId,Sum,CurrencyId,SumInCurrency,CurrencyRate,ImportanceCategoryId,Date")] Spending spending)
        {

            if (id != spending.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    spending.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).Select(c => c.Id).First();
                    await _context.EditwithCheckCurrency(spending);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpendingExists(spending.Id))
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
            return View(spending);
        }

        // GET: Spendings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spending = await _context.Spendings
                .Include(s => s.ImportanceCategory)
                .Include(s => s.SpendingCategory)
                .Include(s => s.SpendingSubCategory)
                // .Include(s => s.Unit)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (spending == null)
            {
                return NotFound();
            }

            return View(spending);
        }

        // POST: Spendings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var spending = await _context.Spendings.FindAsync(id);
            _context.Spendings.Remove(spending);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpendingExists(int id)
        {
            return _context.Spendings.Any(e => e.Id == id);
        }

        public ActionResult GetSpendingSubCategoriesById(int id)
        {
            List<SpendingSubCategory> SubCategoryId = _context.SpendingSubCategories.Where(x => x.SpendingCategoryId == id).ToList();
            return Json(SubCategoryId, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }

        public async Task FillSelectListsAsync(SpendingsViewModel SpendigsListView)
        {
            var ImportanceCategoriesId = new SelectList(await _context.ImportanceCategories.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            var SpendingCategory = _context.SpendingCategories;
            var SpendingCategories = new SelectList(await _context.SpendingCategories.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            SelectList SpendingSubCategories;

            if (SpendigsListView.SpendingSubCategoryId == 0)
            {
                SpendingSubCategories = new SelectList(await _context.SpendingSubCategories
                   .Where(c => c.User.Login == User.Identity.Name && c.SpendingCategoryId == SpendingCategory
                   .FirstOrDefault().Id)
                   .OrderBy(c => c.Id)
                   .ToListAsync(), "Id", "Name", SpendigsListView.SpendingSubCategoryId);
            }
            else
            {
                SpendingSubCategories = new SelectList(await _context.SpendingSubCategories
                 .Where(c => c.User.Login == User.Identity.Name && c.Id == SpendigsListView.SpendingSubCategoryId)
                 .OrderBy(c => c.Id)
                 .ToListAsync(), "Id", "Name", SpendigsListView.SpendingSubCategoryId);
            }
            var Currencies = new SelectList(await _context.Currencies.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            SpendigsListView.ImportanceCategoriesId = ImportanceCategoriesId;
            SpendigsListView.SpendingCategories = SpendingCategories;
            SpendigsListView.SpendingSubCategories = SpendingSubCategories;
            SpendigsListView.Currencies = Currencies;

        }
    }
}
