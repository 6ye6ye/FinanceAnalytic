using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.Interfaces;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Models.ViewModels.AnalyticHelpersModels;
using static FinanceAnalytic.Models.ViewModels.AllUsersStatistics;

namespace FinanceAnalytic.Controllers.Helpers
{
    public static class AnalyticHelper
    {

        public static List<GroupedList> GroupByDateTypeWithCategory<T>(int PeriodType, List<T> list) where T : IHasDate, IHasCategory, IHasSum
        {
            switch (PeriodType)
            {
                case 0:
                    return list
                        .GroupBy(g => new { g.Date, g.CategoryName })
                        .Select(p => new GroupedList { Date = p.Key.Date, DateString = p.Key.Date.ToShortDateString(), Sum = p.Sum(g => g.Sum),  CategoryName = p.Key.CategoryName }).ToList();

                case 1:
                    return list
                    .GroupBy(g => new { g.Date.Month, g.Date.Year, g.CategoryName })
                     .Select(p => new GroupedList { 
                         Date = new DateTime(p.Key.Year, p.Key.Month, 1), 
                         DateString = p.Key.Year.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(p.Key.Month), 
                         Sum = p.Sum(g => g.Sum), 
                         CategoryName = p.Key.CategoryName ,
                     }).ToList();

                case 2:
                    return list
                    .GroupBy(g => new { DateTime.Parse(g.Date.ToString()).Year, g.CategoryName })
                    .Select(p => new GroupedList { DateString = p.Key.Year.ToString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.CategoryName }).ToList();

                default:
                    return list
                    .GroupBy(g => new { Date = g.Date, g.CategoryName })
                    .Select(p => new GroupedList { DateString = p.Key.Date.ToShortDateString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.CategoryName }).ToList();
            }
        }



        public static List<GroupedList> GroupByDateType<T>(int PeriodType, List<T> list) where T : IHasDate, IHasSum
        {
            switch (PeriodType)
            {
                case 0:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date })
                        .Select(p => new GroupedList { Date = p.Key.Date, 
                            DateString = p.Key.Date.ToShortDateString(), 
                            Sum = p.Sum(g => g.Sum)
                             })
                        .ToList();

                case 1:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date.Month, g.Date.Year })
                        .Select(p => new GroupedList { 
                            Date = new DateTime(p.Key.Year, p.Key.Month, 1), 
                            DateString = p.Key.Year.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(p.Key.Month), 
                            Sum = p.Sum(g => g.Sum)})
                        .ToList();

                case 2:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { DateTime.Parse(g.Date.ToString()).Year })
                        .Select(p => new GroupedList { 
                            Date = new DateTime(p.Key.Year, 1, 1), 
                            DateString = p.Key.Year.ToString(), 
                            Sum = p.Sum(g => g.Sum)
                        })
                        .ToList();
                default:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date })
                        .Select(p => new GroupedList {
                            DateString = p.Key.Date.ToShortDateString(), 
                            Sum = p.Sum(g => g.Sum), 
                            })
                        .ToList();
            }
        }


        public static List<GoalsAnalytic> GetGoalsAnalyticListAsync(string Login, bool HideAchived, AppDbContext _context)
        {
            //var goalSum = _context.Goals.Where(c => c.Id == GoalId).Select(c => c.Sum).FirstOrDefault();

            var goalsRecordslist = _context.Accumulations.Include(c => c.Goal)
              .Where(c => c.User.Login == Login)
              .OrderBy(c => c.Goal.Name)
              .AsEnumerable();
            if (HideAchived)
            {
                goalsRecordslist = goalsRecordslist.Where(c => c.Goal.IsAchived == false).ToList();
            }


            return goalsRecordslist.GroupBy(c => new { c.Goal })
               .Select(c => new GoalsAnalytic()
               {
                   GoalName = c.Key.Goal.Name,
                   CurrentSum = c.Sum(s => s.Sum),
                   GoalSum = c.Key.Goal.Sum,

               }).ToList();

        }

        public static async Task<List<OneGoalAnalytic>> GetOneGoalListByPeriodTypeAsync(int PeriodType, int GoalId, string Login, AppDbContext _context)
        {
            var list = _context.Accumulations.Include(c => c.Goal)
                    .Where(c => c.User.Login == Login && c.GoalId == GoalId)
                    .OrderBy(c => c.Date);

            var goalSum = _context.Goals.Where(c => c.Id == GoalId).Select(c => c.Sum).FirstOrDefault();
            var oneGoalAnalyticList = new List<OneGoalAnalytic>();
            switch (PeriodType)
            {
                case 0:
                    oneGoalAnalyticList = await list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date })
                        .Select(p => new OneGoalAnalytic { Date = p.Key.Date, DateString = p.Key.Date.ToShortDateString(), CurrentSum = p.Sum(g => g.Sum) }).ToListAsync();
                    break;
                case 1:
                    oneGoalAnalyticList = await list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date.Month, g.Date.Year })
                        .Select(p => new OneGoalAnalytic { Date = new DateTime(p.Key.Year, p.Key.Month, 1), DateString = p.Key.Year.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(p.Key.Month), CurrentSum = p.Sum(g => g.Sum) }).ToListAsync();
                    break;
                case 2:
                    oneGoalAnalyticList = await list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { DateTime.Parse(g.Date.ToString()).Year })
                        .Select(p => new OneGoalAnalytic { Date = new DateTime(p.Key.Year, 1, 1), DateString = p.Key.Year.ToString(), CurrentSum = p.Sum(g => g.Sum) }).ToListAsync();
                    break;
                default:
                    oneGoalAnalyticList = await list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date })
                        .Select(p => new OneGoalAnalytic { DateString = p.Key.Date.ToShortDateString(), CurrentSum = p.Sum(g => g.Sum) }).ToListAsync();
                    break;
            }
            decimal sum = 0;
            foreach (var item in oneGoalAnalyticList)
            {
                sum += item.CurrentSum;
                item.AccumulationSum = sum;

            }
            return oneGoalAnalyticList;
        }

        public static async Task<List<CountByDate>> GetUsersCountListByPeriodType(int PeriodType, DateTime PeriodBegin, DateTime PeriodEnd, AppDbContext _context)
        {
            var list = await _context.Users
                .Where(s => s.RegistrationDate >= PeriodBegin && s.RegistrationDate <= PeriodEnd.AddDays(1))
                .GroupBy(c => c.RegistrationDate.Date)
                .Select(c => new CountByDate() { Date = c.Key.Date, Count = c.Count() }).ToListAsync();

            return  GroupCountByDate(PeriodType, list);
        }

        public static async Task<List<CountByDate>> GetCountListByPeriodType<T>(int PeriodType, DateTime PeriodBegin, DateTime PeriodEnd, AppDbContext _context, T entityType) where T : class, IHasDate
        {
            var list = await _context.Set<T>()
               .Where(s => s.Date >= PeriodBegin && s.Date <= PeriodEnd.AddDays(1))
               .GroupBy(c => c.Date.Date)
               .Select(c => new CountByDate() { Date = c.Key.Date, Count = c.Count() }).ToListAsync();
            return  GroupCountByDate(PeriodType, list);
        }
        public static List<CountByDate> GroupCountByDate(int PeriodType, List<CountByDate> list)
        {
            switch (PeriodType)
            {
                case 0:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date })
                        .Select(p => new CountByDate { Date = p.Key.Date, DateString = p.Key.Date.ToShortDateString(), Count = p.Sum(g => g.Count) }).ToList();

                case 1:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date.Month, g.Date.Year })
                        .Select(p => new CountByDate { Date = new DateTime(p.Key.Year, p.Key.Month, 1), DateString = p.Key.Year.ToString() + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(p.Key.Month), Count = p.Sum(g => g.Count) }).ToList();

                case 2:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { DateTime.Parse(g.Date.ToString()).Year })
                        .Select(p => new CountByDate { Date = new DateTime(p.Key.Year, 1, 1), DateString = p.Key.Year.ToString(), Count = p.Sum(g => g.Count) }).ToList();

                default:
                    return list
                        .OrderBy(g => g.Date)
                        .GroupBy(g => new { g.Date })
                        .Select(p => new CountByDate { DateString = p.Key.Date.ToShortDateString(), Count = p.Sum(g => g.Count) }).ToList();
            }
        }
        public static List<GroupedList> ConvertToGroupedListIncomesListByCategories(List<Income> list)
        {
            return list
               .GroupBy(c => new { c.Date, c.IncomeCategory.Name })
               .Select(p => new GroupedList { Date = p.Key.Date, Sum = p.Sum(g => g.Sum), CategoryName = p.Key.Name })
               .OrderBy(c => c.Date)
               .ToList();
        }
        public static List<GroupedList> ConvertToGroupedListSpendingsListByCategories(List<Spending> list)
        {
            return list
               .GroupBy(c => new { c.Date, c.SpendingCategory.Name })
               .Select(p => new GroupedList { Date = p.Key.Date, Sum = p.Sum(g => g.Sum),  CategoryName = p.Key.Name })
               .OrderBy(c => c.Date)
               .ToList();
        }

        public static List<Income> GetIncomesListWithFilters(string login, IncomesAnalyticViewModel viewModel, AppDbContext _context)
        {
            return _context.Incomes.Include(p => p.IncomeCategory)
                .Include(p => p.IncomeCategory)
                .Where(p => (p.User.Login == login)
                                  && (viewModel.SelectedIncomeSubCategory == 0 || p.IncomeSubCategoryId == viewModel.SelectedIncomeSubCategory)
                                  && (viewModel.SelectedIncomeCategory == 0 || p.IncomeCategoryId == viewModel.SelectedIncomeCategory)
                                  && ((viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue) || p.Date >= viewModel.PeriodBegin && p.Date <= viewModel.PeriodEnd)).ToList();
        }
        public static List<Spending> GetSpendingsListWithFilters<T>(string login, T viewModel, AppDbContext _context) where T : IHasDateBeginEnd, IHasSelectedItemsForSpendings
        {
            return _context.Spendings
               .Include(p => p.SpendingCategory)
               .Include(p => p.SpendingCategory)
               .Where(p => (p.User.Login == login)
                               && (viewModel.SelectedImportanceCategory == 0 || p.ImportanceCategoryId == viewModel.SelectedImportanceCategory)
                               && (viewModel.SelectedSpendingSubCategory == 0 || p.SpendingSubCategoryId == viewModel.SelectedSpendingSubCategory)
                               && (viewModel.SelectedSpendingCategory == 0 || p.SpendingCategoryId == viewModel.SelectedSpendingCategory)
                               && ((viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue) || p.Date >= viewModel.PeriodBegin && p.Date <= viewModel.PeriodEnd)).ToList();
        }

        public static IQueryable<Spending> GetSpendingsQueryableWithFilters(string login, SpendingsListViewModel viewModel, AppDbContext _context)
        {
            return _context.Spendings.Include(p => p.SpendingCategory).Include(p => p.SpendingSubCategory).Include(p => p.Currency).Where(p => (p.User.Login == login) && (viewModel.SelectedImportanceCategory == 0 || p.ImportanceCategoryId == viewModel.SelectedImportanceCategory)
                                   && (viewModel.SelectedSpendingSubCategory == 0 || p.SpendingSubCategoryId == viewModel.SelectedSpendingSubCategory)
                                   && (viewModel.SelectedSpendingCategory == 0 || p.SpendingCategoryId == viewModel.SelectedSpendingCategory)
                                   && ((viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue) || p.Date >= viewModel.PeriodBegin && p.Date <= viewModel.PeriodEnd)).AsQueryable();
        }
        public static IQueryable<Income> GetIncomesQueryableWithFilters(string login, IncomeListViewModel viewModel, AppDbContext _context)
        {
            return _context.Incomes.Include(p => p.IncomeCategory).Include(p => p.IncomeSubCategory).Include(p => p.Currency).Where(p => (p.User.Login == login)
                                 && (viewModel.SelectedIncomeSubCategory == 0 || p.IncomeSubCategoryId == viewModel.SelectedIncomeSubCategory)
                                 && (viewModel.SelectedIncomeCategory == 0 || p.IncomeCategoryId == viewModel.SelectedIncomeCategory)
                                 && ((viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue) || p.Date >= viewModel.PeriodBegin && p.Date <= viewModel.PeriodEnd)).AsQueryable();
        }
        internal static IQueryable<Accumulation> GetAccumulationsQueryableWithFilters(string login, AccumulationsListViewModel viewModel, AppDbContext _context)
        {
            return _context.Accumulations.Include(p => p.Goal).Where(p => (p.User.Login == login)
                                && (viewModel.SelectedGoal == 0 || p.GoalId == viewModel.SelectedGoal)
                                && (!viewModel.HideAchived || p.Goal.IsAchived != viewModel.HideAchived)
                                && ((viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue) || p.Date >= viewModel.PeriodBegin && p.Date <= viewModel.PeriodEnd)).AsQueryable();

        }

        public static List<Spending> GetSpendingsListWithFilterByDate(string login, DateTime PeriodBegin, DateTime PeriodEnd, AppDbContext _context)
        {
            return _context.Spendings
                .Where(p => (p.User.Login == login)
                && ((PeriodBegin == DateTime.MinValue && PeriodEnd == DateTime.MinValue)
                || p.Date >= PeriodBegin
                && p.Date <= PeriodEnd)).ToList();
        }
        public static List<Income> GetIncomesListWithFilterByDate(string login, DateTime PeriodBegin, DateTime PeriodEnd, AppDbContext _context)
        {
            return _context.Incomes.Where(p => (p.User.Login == login)
                && ((PeriodBegin == DateTime.MinValue && PeriodEnd == DateTime.MinValue)
                || p.Date >= PeriodBegin
                && p.Date <= PeriodEnd)).ToList();
        }


        //public static IQueryable<Spending> GetSpendingsIQueryableWithFilters(string login, SpendingsListViewModel viewModel, AppDbContext _context)
        //{

        //    return _context.Spendings.Include(p => p.SpendingCategory).Include(p => p.SpendingSubCategory).Where(p => (p.User.Login == login) && (viewModel.SelectedImportanceCategory == 0 || p.ImportanceCategoryId == viewModel.SelectedImportanceCategory)
        //                          && (viewModel.SelectedSpendingSubCategory == 0 || p.SpendingSubCategoryId == viewModel.SelectedSpendingSubCategory)
        //                          && (viewModel.SelectedSpendingCategory == 0 || p.SpendingCategoryId == viewModel.SelectedSpendingCategory)
        //                          && ((viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue) || p.Date >= viewModel.PeriodBegin && p.Date <= viewModel.PeriodEnd));

        //}

        //public static List<GroupedListByCategories> GroupSpendingsByDateType(int PeriodType, List<GroupedListByCategories> Spendings) 
        //{

        //    var GroupSpendings = new List<GroupedListByCategories>();
        //    switch (PeriodType)
        //    {
        //        case 0:
        //            GroupSpendings = Spendings
        //                .GroupBy(g => new { g.Date,  g.CategoryName })
        //                .Select(p => new GroupedListByCategories { DateString = p.Key.Date.ToShortDateString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.CategoryName }).ToList();
        //            break;

        //        case 1:
        //            GroupSpendings = Spendings
        //             .GroupBy(g => new { g.Date.Month, g.Date.Year, g.CategoryName })
        //             .Select(p => new GroupedListByCategories { DateString = p.Key.Year.ToString() + " " + p.Key.Month.ToString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.CategoryName }).ToList();
        //            break;
        //        case 2:
        //            GroupSpendings = Spendings
        //              .GroupBy(g => new { DateTime.Parse(g.Date.ToString()).Year,  g.CategoryName })
        //            .Select(p => new GroupedListByCategories { DateString = p.Key.Year.ToString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.CategoryName }).ToList();
        //            break;
        //        default:
        //            GroupSpendings = Spendings
        //                .GroupBy(g => new { Date = g.Date,  g.CategoryName })
        //                .Select(p => new GroupedListByCategories { DateString = p.Key.Date.ToShortDateString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.CategoryName }).ToList();
        //            break;
        //    }
        //    return GroupSpendings;

        //}
        //public static List<GroupedListByCategories> GroupListWithCategoriesByDateType(int PeriodType, List<GroupedListByCategories> list)
        //{

        //    var GroupSpendings = new List<GroupedListByCategories>();
        //    switch (PeriodType)
        //    {
        //        case 0:
        //            GroupSpendings = list
        //                .GroupBy(g => new { g.Date, IncomeCategory = g.CategoryName })
        //                .Select(p => new GroupedListByCategories { DateString = p.Key.Date.ToShortDateString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.IncomeCategory }).ToList();
        //            break;

        //        case 1:
        //            GroupSpendings = list
        //             .GroupBy(g => new { g.Date.Month, g.Date.Year, g.CategoryName })
        //             .Select(p => new GroupedListByCategories { DateString = p.Key.Year.ToString() + " " + p.Key.Month.ToString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.CategoryName }).ToList();
        //            break;
        //        case 2:
        //            GroupSpendings = list
        //              .GroupBy(g => new { DateTime.Parse(g.Date.ToString()).Year, IncomeCategory = g.CategoryName })
        //            .Select(p => new GroupedListByCategories { DateString = p.Key.Year.ToString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.IncomeCategory }).ToList();
        //            break;
        //        default:
        //            GroupSpendings = list
        //                .GroupBy(g => new { Date = g.Date, IncomeCategory = g.CategoryName })
        //                .Select(p => new GroupedListByCategories { DateString = p.Key.Date.ToShortDateString(), Sum = p.Sum(g => g.Sum), CategoryName = p.Key.IncomeCategory }).ToList();
        //            break;
        //    }
        //    return GroupSpendings;

        //}



    }
}
