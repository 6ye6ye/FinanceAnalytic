using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models.Interfaces;
using System.Text.Json;

namespace FinanceAnalytic.Models.ViewModels
{
    public class SpendingsListViewModel : IHasDateBeginEnd,IValidatableObject
    {
        [Display(Name = "Наименование")]
        public IEnumerable<Spending> Spendings { get; set; }
        public Spending SpendingParametr { get; set; }

       
        [Display(Name = "Категория расходов")]
        public SelectList SpendingCategory { get; set; }
        public int SelectedSpendingCategory { get; set; }

        [Display(Name = "Подкатегория расходов")]
        public SelectList SpendingSubCategory { get; set; }
        public int SelectedSpendingSubCategory { get; set; }
       
        [Display(Name = "Категория важности")]
        public SelectList ImportanceCategory { get; set; }
        public int SelectedImportanceCategory { get; set; }
        
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
