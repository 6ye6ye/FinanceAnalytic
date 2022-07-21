using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize]
    public class IncomesController : Controller
    {
        private readonly AppDbContext _context;

        public IncomesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Incomes
       public async Task<IActionResult> Index(IncomeListViewModel viewModel)
   {
            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }
            var incomesList = AnalyticHelper.GetIncomesQueryableWithFilters(User.Identity.Name, viewModel, _context);
            ViewBag.SortDateParametr = string.IsNullOrEmpty(viewModel.SortParametr) ? "Date desc" : "";
            ViewBag.SortIncomeCategoryParametr = viewModel.SortParametr == "IncomeCategory" ? "IncomeCategory desc" : "IncomeCategory";
            ViewBag.SortIncomeSubCategoryParametr = viewModel.SortParametr == "IncomeSubCategory" ? "IncomeSubCategory desc" : "IncomeSubCategory";
            ViewBag.SortSumParametr = viewModel.SortParametr == "Sum" ? "Sum desc" : "Sum";

            switch (viewModel.SortParametr)
            {
                case "Date desc":
                    incomesList = incomesList.OrderByDescending(x => x.Date);
                    break;
                case "IncomeCategory desc":
                    incomesList = incomesList.OrderByDescending(x => x.IncomeCategory.Name);
                    break;
                case "IncomeCategory":
                    incomesList = incomesList.OrderBy(x => x.IncomeCategory.Name);
                    break;
                case "IncomeSubCategory desc":
                    incomesList = incomesList.OrderByDescending(x => x.IncomeSubCategory.Name);
                    break;
                case "IncomeSubCategory":
                    incomesList = incomesList.OrderBy(x => x.IncomeSubCategory.Name);
                    break;
            
                case "Sum desc":
                    incomesList = incomesList.OrderByDescending(x => x.Sum);
                    break;
                case "Sum":
                    incomesList = incomesList.OrderBy(x => x.Sum);
                    break;
                default:
                    incomesList = incomesList.OrderBy(x => x.Date);
                    break;
            }
            var incomeCategories = _context.IncomeCategories
              .OrderBy(c => c.Name)
              .ToList();
            var incomesSubCategories = _context.IncomeSubCategories
             .OrderBy(c => c.Name)
             .Where(c => (c.IncomeCategoryId == viewModel.SelectedIncomeCategory && c.User.Login==User.Identity.Name))
             .ToList();
            viewModel.Incomes = incomesList.ToList();
            viewModel.IncomeCategory = new SelectList(incomeCategories, "Id", "Name");
            viewModel.IncomeSubCategory = new SelectList(incomesSubCategories, "Id", "Name");
            return View(viewModel);
        }

        // GET: Incomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.IncomeSubCategory)
                .Include(i => i.IncomeCategory)
                .Include(i => i.User)
                .Include(i => i.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }
        public async Task FillSelectListsAsync(IncomesViewModel viewModel)
        {
            var Category = _context.IncomeCategories;
            var IncomeCategories = new SelectList(await _context.IncomeCategories.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            
            SelectList IncomeSubCategories;
            if (viewModel.IncomeSubCategoryId == 0)
            {
                 IncomeSubCategories = new SelectList(await _context.IncomeSubCategories
                    .Where(c => c.User.Login == User.Identity.Name && c.IncomeCategoryId == Category
                    .FirstOrDefault().Id)
                    .OrderBy(c => c.Id)
                    .ToListAsync(), "Id", "Name");
            }
            else
            {
                IncomeSubCategories = new SelectList(await _context.IncomeSubCategories
                  .Where(c => c.Id == viewModel.IncomeSubCategoryId)
                  .OrderBy(c => c.Id)
                    .ToListAsync(), "Id", "Name", viewModel.IncomeSubCategoryId);
            }


                var Currencies = new SelectList(await _context.Currencies.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            
            viewModel.IncomeCategories = IncomeCategories;
            viewModel.IncomeSubCategories = IncomeSubCategories;
            viewModel.Currencies = Currencies;
            
        }
        // GET: Incomes/Create
        public async Task<IActionResult> Create()
        {
            var IncomesViewModel = new IncomesViewModel { Date = DateTime.Now };
            IncomesViewModel.UserCurrencyName = _context.Users.Where(u => u.Login == User.Identity.Name).Select(u => u.Currency.Name).FirstOrDefault();
            await FillSelectListsAsync(IncomesViewModel);
            return View(IncomesViewModel);
        }

        // POST: Incomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncomesViewModel incomesListView)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    incomesListView.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                    await _context.AddWithCheckCurrency(incomesListView.Model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            await FillSelectListsAsync(incomesListView);
            return View(incomesListView);
        }

        // GET: Incomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            var IncomesViewModel = new IncomesViewModel(income);
            await FillSelectListsAsync(IncomesViewModel);
            return View(IncomesViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IncomesViewModel incomesListView)
        {
            if (id != incomesListView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    incomesListView.Model.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                    _context.Update(incomesListView.Model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(incomesListView.Id))
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
            return View(incomesListView);
        }

        // GET: Incomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.IncomeSubCategory)
                .Include(i => i.IncomeCategory)
                .Include(i => i.User)
                .Include(i => i.Currency)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var income = await _context.Incomes.FindAsync(id);
            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomeExists(int id)
        {
            return _context.Incomes.Any(e => e.Id == id);
        }
    }
}
