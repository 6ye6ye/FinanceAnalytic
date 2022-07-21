using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FinanceAnalytic.Models.ViewModels
{
    public class IncomeListViewModel : IHasDateBeginEnd,IValidatableObject
    {

        [Display(Name = "Наименование")]
        public IEnumerable<Income> Incomes { get; set; }

        public Income IncomeParametr { get; set; }

        [Display(Name = "Категория доходов")]
        public SelectList IncomeCategory { get; set; }
        public int SelectedIncomeCategory { get; set; }

        [Display(Name = "Подкатегория доходов")]
        public SelectList IncomeSubCategory { get; set; }
        public int SelectedIncomeSubCategory { get; set; }

        [Display(Name = "Начало периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodBegin { get; set; }

        [Display(Name = "Конец периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }

        [Display(Name = "Дата")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string SortParametr { get; set; }

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
    }
}
