using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Models.ViewModels.AnalyticHelpersModels;

namespace FinanceAnalytic.Controllers.Helpers
{
    public class DataForChartsHelper : Controller
    {
        private readonly AppDbContext _context;


        public DataForChartsHelper(AppDbContext context)
        {
            _context = context;

        }

        public ActionResult GetPieChartDataIncomesCat(List<GroupedList> list)
        {
            if (list.Count() == 0) return Json(null);

            var rezultList = list.GroupBy(g => g.CategoryName).Select(s => new
            {
                name = s.Key.Trim(),
                cost = s.Sum(i => i.Sum)
            }).ToList();
            return Json(rezultList);
        }

        [HttpPost]
        public ActionResult GetChartDataForUserRegistrAnalytic(List<UsersCountAnalytic> list)
        {
            List<object> chartData = new List<object>();
            foreach (var item in list)
            {
                List<object> arrayRow = new List<object>();
                arrayRow.Add(item.Date.ToString("yyyy-MM-dd"));
                arrayRow.Add(item.Count);
                chartData.Add(arrayRow.ToArray());
            }
            return Json(chartData);
        }

        [HttpPost]
        public ActionResult GetChartDataForOneGoal(List<OneGoalAnalytic> list, decimal goalSum)
        {

            List<object> chartData = new List<object>();
            decimal sum = 0;
            var title = new List<string>();
            title.Add("Дата");
            title.Add("Сумма накопленная");
            title.Add("Цель");
            chartData.Add(title.ToArray());
            foreach (var item in list)
            {
                sum += item.CurrentSum;

                List<object> arrayRow = new List<object>();
                arrayRow.Add(item.DateString);
                arrayRow.Add(item.AccumulationSum);
                arrayRow.Add(goalSum);
                chartData.Add(arrayRow.ToArray());
            }
            return Json(chartData);
        }

        [HttpPost]
        public ActionResult GetColumnChartDataForGoals(List<GoalsAnalytic> list)
        {
            List<object> chartData = new List<object>();

            var title = new List<string>();
            title.Add("Цель");
            title.Add("Сумма накопленная");
            title.Add("Сумма нехватающая");
            chartData.Add(title.ToArray());

            foreach (var item in list)
            {
                List<object> arrayRow = new List<object>();
                arrayRow.Add(item.GoalName);
                arrayRow.Add((decimal)item.CurrentSum);
                arrayRow.Add((decimal)item.GoalSum - item.CurrentSum);
                arrayRow.Add((decimal)item.GoalSum);
                chartData.Add(arrayRow.ToArray());
            }
            return Json(chartData);
        }

        [HttpPost]
        public ActionResult GetColumnChartDataForGeneralAnalytic(List<CompareSpendingsAndIncomes> viewModel)
        {
            List<object> chartData = new List<object>();

            var title = new List<string>();
            title.Add("Дата");
            title.Add("Доходы");
            title.Add("Расходы");

            chartData.Add(title.ToArray());

            foreach (var item in viewModel)
            {
                List<object> arrayRow = new List<object>();
                arrayRow.Add(item.DateString);
                arrayRow.Add(item.IncomeSum);
                arrayRow.Add(item.SpendingsSum);
                chartData.Add(arrayRow.ToArray());
            }
            return Json(chartData);
        }

        [HttpPost]
        public ActionResult GetColumnChartDataIncomes(List<GroupedList> list)
        {
            var dt = new DataTable();

            dt = DataTableConverter.ToDataTable(list);

            List<object> chartData = new List<object>();
            List<string> IncomeCategories = list
                .Select(s => s.CategoryName)
                .Distinct()
                .ToList();

            IncomeCategories.Insert(0, "Категория расходов");
            chartData.Add(IncomeCategories.ToArray());
            List<string> dates = (from p in dt.AsEnumerable()
                                  select p.Field<string>("DateString")).Distinct().ToList();
            foreach (var date in dates)
            {
                List<object> arrayRow = new List<object>();
                arrayRow.Add(date);
                for (int i = 1; i < IncomeCategories.Count; i++)
                {

                    var my = (from p in dt.AsEnumerable()
                              where p.Field<string>("DateString") == date
                              where p.Field<string>("CategoryName") == IncomeCategories[i]
                              select p.Field<decimal>("Sum")).FirstOrDefault();
                    arrayRow.Add(my);
                }
                chartData.Add(arrayRow.ToArray());
            }
            return Json(chartData);
        }
        [HttpPost]
        public ActionResult GetColumnChartDataSpendings(List<GroupedList> groupedList)
        {
            var dt = new DataTable();

            dt = DataTableConverter.ToDataTable(groupedList);

            List<object> chartData = new List<object>();
            List<string> SpendingCategories = groupedList
                .Select(s => s.CategoryName)
                .Distinct()
                .ToList();

            SpendingCategories.Insert(0, "Категория расходов");
            chartData.Add(SpendingCategories.ToArray());
            List<string> dates = (from p in dt.AsEnumerable()
                                  select p.Field<string>("DateString")).Distinct().ToList();
            foreach (var date in dates)
            {
                List<object> arrayRow = new List<object>();
                arrayRow.Add(date);
                for (int i = 1; i < SpendingCategories.Count; i++)
                {

                    var my = (from p in dt.AsEnumerable()
                              where p.Field<string>("DateString") == date
                              where p.Field<string>("CategoryName") == SpendingCategories[i]
                              select p.Field<decimal>("Sum")).FirstOrDefault();
                    arrayRow.Add(my);
                }
                chartData.Add(arrayRow.ToArray());
            }
            return Json(chartData);
        }


        public ActionResult GetPieChartDataSpendingImp(DateTime PeriodBegin, DateTime PeriodEnd)
        {
            // var list = _context.Spendings.Where(c => c.User.Login == User.Identity.Name).GroupBy(g => g.ImportanceCategory.Name).Select(s => new { Name= s.Key.Trim(), cost = s.Sum(i => i.cost), date = s.}).AsQueryable().ToList();

            ImportanceCategory impCategory = new ImportanceCategory();
            impCategory.Name = "Не выбранно";
            impCategory.NumericEquivalent = 0;
            impCategory.Id = 0;
            var list = _context.Spendings
                .Where(c => c.User.Login == User.Identity.Name && c.Date >= PeriodBegin && c.Date <= PeriodEnd)
                .Select(s => new { s.ImportanceCategory, s.Sum, s.Date })
                .GroupBy(s => s.ImportanceCategory.Name)
                .Select(s => new
                {
                    cost = s.Sum(i => i.Sum),
                    name = s.Key == null ? "Не указано" : s.Key
                }).ToList();

            if (list.Count() == 0) return Json(null);
            return Json(list);

        }
        public ActionResult GetPieChartDataSpendingCat(DateTime PeriodBegin, DateTime PeriodEnd)
        {
            if (User.Identity.IsAuthenticated)
            {
                var list = _context.Spendings.Where(c => c.User.Login == User.Identity.Name).Select(s => new { s.SpendingCategory.Name, s.Sum, s.Date }).OrderBy(c => c.Date).ToList().AsQueryable();
                if (PeriodBegin != DateTime.MinValue && PeriodEnd != DateTime.MinValue)
                {
                    list = list.Where(p => p.Date >= PeriodBegin && p.Date <= PeriodEnd);
                }
                if (list.Count() == 0) return Json(null);
                var rezultList = list.GroupBy(g => g.Name).Select(s => new
                {
                    cost = s.Sum(i => i.Sum),
                    name = s.Key.Trim() + " " + s.Sum(i => i.Sum),
                }).ToList();
                return Json(rezultList);
            }
            else return Json(null);
        }

    }

}

