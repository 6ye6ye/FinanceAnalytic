using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize]
    public class AccumulationsController : Controller
    {
        private readonly AppDbContext _context;

        public AccumulationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: GoalRecords
        public async Task<IActionResult> Index(AccumulationsListViewModel viewModel, string sortBy)
        {
            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }
            var accumulationsList = AnalyticHelper.GetAccumulationsQueryableWithFilters(User.Identity.Name, viewModel, _context);
            ViewBag.SortDateParametr = string.IsNullOrEmpty(viewModel.SortParametr) ? "Date desc" : "";
            ViewBag.SortGoalParametr = viewModel.SortParametr == "Goal" ? "Goal desc" : "Goal";
            ViewBag.SortSumParametr = viewModel.SortParametr == "Sum" ? "Sum desc" : "Sum";

            switch (viewModel.SortParametr)
            {
                case "Date desc":
                    accumulationsList = accumulationsList.OrderByDescending(x => x.Date);
                    break;
                case "Goal desc":
                    accumulationsList = accumulationsList.OrderByDescending(x => x.Goal.Name);
                    break;
                case "Goal":
                    accumulationsList = accumulationsList.OrderBy(x => x.Goal.Name);
                    break;


                case "Sum desc":
                    accumulationsList = accumulationsList.OrderByDescending(x => x.Sum);
                    break;
                case "Sum":
                    accumulationsList = accumulationsList.OrderBy(x => x.Sum);
                    break;
                default:
                    accumulationsList = accumulationsList.OrderBy(x => x.Date);
                    break;
            }
            var goalsList = _context.Goals
                .Where(c => c.User.Login == User.Identity.Name)
              .OrderBy(c => c.Name)
              .ToList();

            viewModel.Accumulations = accumulationsList.ToList();
            viewModel.Goals = new SelectList(goalsList, "Id", "Name");

            return View(viewModel);
    
        }
        public async Task FillSelectListsAsync(AccumulationViewModel viewModel)
        {
            
            var goals = new SelectList(await _context.Goals.Where(c=>c.User.Login==User.Identity.Name && c.IsAchived==false).OrderBy(c => c.Id).ToListAsync(), "Id", "Name");
            var Currencies = new SelectList(await _context.Currencies.OrderBy(c => c.Id).ToListAsync(), "Id", "Name");

            viewModel.Goals = goals;
            viewModel.Currencies = Currencies;
        }
        // GET: GoalRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accumulation = await _context.Accumulations
                .Include(g => g.Goal)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accumulation == null)
            {
                return NotFound();
            }

            return View(accumulation);
        }

        // GET: GoalRecords/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new AccumulationViewModel();
            viewModel.Date = DateTime.Now;
            await FillSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // POST: GoalRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccumulationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                await _context.AddWithCheckCurrency(viewModel.Model);
                return RedirectToAction(nameof(Index));
            }
         
            await FillSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // GET: GoalRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accumulation = await _context.Accumulations.FindAsync(id);
            if (accumulation == null)
            {
                return NotFound();
            }
            var viewModel = new AccumulationViewModel(accumulation);
            await FillSelectListsAsync(viewModel);
            return View(viewModel);
        }


        // POST: GoalRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AccumulationViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    viewModel.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                    _context.Update(viewModel.Model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalRecordExists(viewModel.Id))
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
            await FillSelectListsAsync(viewModel);
            return View(viewModel);
        }

        // GET: GoalRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accumulation = await _context.Accumulations
                .Include(g => g.Goal)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accumulation == null)
            {
                return NotFound();
            }

            return View(accumulation);
        }

        // POST: GoalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accumulation = await _context.Accumulations.FindAsync(id);
            _context.Accumulations.Remove(accumulation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoalRecordExists(int id)
        {
            return _context.Accumulations.Any(e => e.Id == id);
        }
    }
}
