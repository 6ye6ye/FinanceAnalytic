using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models.Enums;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers
{
    [Authorize]
    public class IncomesAnalyticViewModelController : Controller
    {
        private readonly AppDbContext _context;


        public IncomesAnalyticViewModelController(AppDbContext context)
        {
            _context = context;

        }

#pragma warning disable CS1998 // В данном асинхронном методе отсутствуют операторы await, поэтому метод будет выполняться синхронно. Воспользуйтесь оператором await для ожидания неблокирующих вызовов API или оператором await Task.Run(...) для выполнения связанных с ЦП заданий в фоновом потоке.
        public async Task<IActionResult> IncomesAnalytic(IncomesAnalyticViewModel viewModel)
#pragma warning restore CS1998 // В данном асинхронном методе отсутствуют операторы await, поэтому метод будет выполняться синхронно. Воспользуйтесь оператором await для ожидания неблокирующих вызовов API или оператором await Task.Run(...) для выполнения связанных с ЦП заданий в фоновом потоке.
        {
            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }
            List<Income> Incomes =  AnalyticHelper.GetIncomesListWithFilters(User.Identity.Name, viewModel, _context);
            var GroupByCategorieIncomes = AnalyticHelper.GroupByDateTypeWithCategory(viewModel.SelectedPeriodTypes, AnalyticHelper.ConvertToGroupedListIncomesListByCategories(Incomes));     
         
            var IncomeSubCategories = _context.IncomeSubCategories
                .OrderBy(c => c.Name)
                .Where(c => c.IncomeCategoryId == viewModel.SelectedIncomeCategory)
                .ToList();
            var IncomeCategories = _context.IncomeCategories
                .OrderBy(c => c.Name)
                .ToList();
            var ImportanceCategoriesId = _context.ImportanceCategories.OrderBy(c => c.Name).ToList();
            var enumData = from PeriodTypesEnum e in Enum.GetValues(typeof(PeriodTypesEnum))
                           select new
                           {
                               Id = (int)e,
                               Name = e.ToString()
                           };
            viewModel.PeriodTypes = new SelectList(enumData.ToList(), "Id", "Name");
            viewModel.IncomesByCategoriesList = GroupByCategorieIncomes;
            viewModel.IncomeCategory = new SelectList(IncomeCategories, "Id", "Name");
            viewModel.IncomeSubCategory = new SelectList(IncomeSubCategories, "Id", "Name");
            viewModel.PeriodTypes = new SelectList(enumData.ToList(), "Id", "Name");
            return View(viewModel);
        }
    }
}
