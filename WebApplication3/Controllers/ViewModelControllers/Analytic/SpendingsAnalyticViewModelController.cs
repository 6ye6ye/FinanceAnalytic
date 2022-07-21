using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models.Enums;
using FinanceAnalytic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers
{
    [Authorize]
    public class SpendingsAnalyticViewModelController : Controller
    {
        private readonly AppDbContext _context;


        public SpendingsAnalyticViewModelController(AppDbContext context)
        {
            _context = context;

        }


        public async Task<IActionResult> SpendingsAnalytic(SpendingsAnalyticViewModel viewModel)
        {

            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }

            var PlanedSpendingsForAnalyticModel = await _context.PlanedSpendings.Where(p => (p.User.Login == User.Identity.Name)
                                && (viewModel.SelectedSpendingSubCategory == 0 || p.SpendingSubCategoryId == viewModel.SelectedSpendingSubCategory)
                                && (viewModel.SelectedSpendingCategory == 0 || p.SpendingCategoryId == viewModel.SelectedSpendingCategory)
                                && ((viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue) || p.PeriodEnd >= viewModel.PeriodBegin && p.PeriodBegin <= viewModel.PeriodEnd))
                                .Select(p => new PlanedSpendingsForAnalyticModel { PeriodBegin = p.PeriodBegin, PeriodEnd = p.PeriodEnd, SpendingCategory = p.SpendingCategory, SpendingSubCategory = p.SpendingSubCategory, SumPlaning = p.Sum }).ToListAsync();

            var Spendings = AnalyticHelper.GetSpendingsListWithFilters(User.Identity.Name, viewModel, _context);
            var GroupByCategorieSpendings = AnalyticHelper.GroupByDateTypeWithCategory(viewModel.SelectedPeriodTypes, AnalyticHelper.ConvertToGroupedListSpendingsListByCategories(Spendings));
            foreach (var item in PlanedSpendingsForAnalyticModel)
            {
                item.Sum = Spendings.Where(s => (item.SpendingSubCategory == null || s.SpendingSubCategory == item.SpendingSubCategory)
                                && (item.SpendingCategory == null || s.SpendingCategory == item.SpendingCategory)
                                 && (s.Date >= item.PeriodBegin && s.Date <= item.PeriodEnd)).Sum(s => s.Sum);
                item.Different = item.SumPlaning - item.Sum;
            }

            viewModel.HasAnyImpCategory = Spendings.Where(c => c.ImportanceCategoryId > 0).Any();
            viewModel.SpendingsByCategoriesList = GroupByCategorieSpendings;
            viewModel.PlanedSpendingsAnalyticList = PlanedSpendingsForAnalyticModel;

            FillSelectList(viewModel);
            return View(viewModel);


        }

        private void FillSelectList(SpendingsAnalyticViewModel viewModel)
        {

            var SpendingSubCategories = _context.SpendingSubCategories
                 .OrderBy(c => c.Name)
                 .Where(c => c.SpendingCategoryId == viewModel.SelectedSpendingCategory)
                 .ToList();
            var spendingCategories = _context.SpendingCategories
                .OrderBy(c => c.Name)
                .ToList();
            var ImportanceCategoriesId = _context.ImportanceCategories
                .OrderBy(c => c.Name)
                .ToList();
            var enumData = from PeriodTypesEnum e in Enum.GetValues(typeof(PeriodTypesEnum))
                           select new
                           {
                               Id = (int)e,
                               Name = e.ToString()
                           };

            viewModel.SpendingCategory = new SelectList(spendingCategories, "Id", "Name");
            viewModel.SpendingSubCategory = new SelectList(SpendingSubCategories, "Id", "Name");
            viewModel.ImportanceCategory = new SelectList(ImportanceCategoriesId, "Id", "Name");

            viewModel.PeriodTypes = new SelectList(enumData.ToList(), "Id", "Name");
        }


    }
}
