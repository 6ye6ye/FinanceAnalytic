using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models.Enums;
using FinanceAnalytic.Models.ViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers
{
    [Authorize]
    public class GeneralAnalyticViewModelController : Controller
    {
        private readonly AppDbContext _context;


        public GeneralAnalyticViewModelController(AppDbContext context)
        {
            _context = context;

        }
        public IActionResult GeneralAnalytic(GeneralAnalyticViewModel viewModel)
        {
            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }

            var incomes = AnalyticHelper.GroupByDateType(viewModel.SelectedPeriodTypes, AnalyticHelper.GetIncomesListWithFilterByDate(User.Identity.Name, viewModel.PeriodBegin,viewModel.PeriodEnd, _context));
            var spendings = AnalyticHelper.GroupByDateType(viewModel.SelectedPeriodTypes, AnalyticHelper.GetSpendingsListWithFilterByDate(User.Identity.Name, viewModel.PeriodBegin, viewModel.PeriodEnd, _context));

            var leftOuterJoin = from income in incomes
                                join spending in spendings on income.DateString equals spending.DateString into ot
                                from otnew in ot.DefaultIfEmpty()
                                select new CompareSpendingsAndIncomes
                                {
                                    DateString = income.DateString,
                                    Date = income.Date,
                                    IncomeSum = income.Sum,
                                    SpendingsSum = otnew == null ? 0 : otnew.Sum

                                };
            var leftOuterJoin1 = leftOuterJoin.ToList();
            var rightOuterJoin = from spending in spendings
                                 join income in incomes on spending.DateString equals income.DateString into ot
                                 from otnew in ot.DefaultIfEmpty()
                                 select new CompareSpendingsAndIncomes
                                 {
                                     DateString = spending.DateString,
                                     Date = spending.Date,
                                     SpendingsSum = spending.Sum,
                                     IncomeSum = otnew == null ? 0 : otnew.Sum

                                 };
            leftOuterJoin = leftOuterJoin.Union(rightOuterJoin);
            var list = leftOuterJoin.OrderBy(c => c.Date)
                .GroupBy(c => c.DateString)
                .Select(c => c.First())
                .Distinct();
            var enumData = from PeriodTypesEnum e in Enum.GetValues(typeof(PeriodTypesEnum))
                           select new
                           {
                               Id = (int)e,
                               Name = e.ToString()
                           };
            viewModel.CompareList = list;
            viewModel.PeriodTypes = new SelectList(enumData.ToList(), "Id", "Name");
            return View(viewModel);
        }
    }
}
