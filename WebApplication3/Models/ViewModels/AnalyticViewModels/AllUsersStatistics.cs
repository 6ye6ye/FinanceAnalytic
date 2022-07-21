using FinanceAnalytic.Models.ViewModels.AnalyticHelpersModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models.ViewModels
{
    public class AllUsersStatistics:IValidatableObject
    {
        //filters----------------------
        [Required(ErrorMessage = "Необходимо указать начало периода")]
        [Display(Name = "Начало периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodBegin { get; set; }

        [Display(Name = "Конец периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }
        [Display(Name = "Вид периода ")]
        public SelectList PeriodTypes { get; set; }
        public int SelectedPeriodTypes { get; set; }



        // Users registration analytic------------------------------
        [Display(Name = "Количество пользователей")]
        public List<CountByDate> UsersRegisterAnalytics { get; set; }

        // Spendings statistic --------------------------------------------
        [Display(Name = "Расходы по категориям")]
        public List<CountByName> SpendingsByCategories { get; set; }
        public int SpendingSubCategoriesCount { get; set; }

        //[Display(Name = "Расходы по датам")]
        //public List<CountByDate> SpendingsCountByDate { get; set; }


        //// Incomes statistic --------------------------------------------
        //[Display(Name = "Доходы по категориям")]
        //public List<CountByName> IncomesByCategories { get; set; }
        //public int IncomesSubCategoriesCount { get; set; }
        //[Display(Name = "Расходы по датам")]
        //public List<CountByDate> IncomesCountByDate { get; set; }


        //// PlanedSpendings statistic --------------------------------------------
        //[Display(Name = "Планирование по категориям")]
        //public List<CountByName> PlanedSpendingsByCategories { get; set; }


        //[Display(Name = "Накопления по датам")]
        //public List<CountByDate> AccumulationCountByDate { get; set; }
        //public int GoalsCount { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (PeriodEnd < PeriodBegin)
            {
                ValidationResult mss = new ValidationResult("Начало периода не может быть пойже конца  периода");
                res.Add(mss);
            }
            return res;
        }

        public class CountByDate
        {
            [BindProperty, DataType(DataType.Date)]
            public DateTime Date { get; set; }
            public string DateString { get; set; }
            public int Count { get; set; }
            public CountByDate(DateTime _Date, string? _DateString, int _Count)
            {
                Date = _Date;
                DateString = _DateString;
                Count = _Count;
            }

            public CountByDate()
            {
            }
        }

        public class CountByName
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public CountByName(string name, int count)
            {
                Name = name;
                Count = count;
            }

        }

    }
}






