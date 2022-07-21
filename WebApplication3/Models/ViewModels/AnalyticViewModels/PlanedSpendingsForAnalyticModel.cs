using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.Enums;

namespace FinanceAnalytic.Models.ViewModels
{
    public class PlanedSpendingsForAnalyticModel:IValidatableObject
    {
        [Display(Name = "Планируемые расходы")]
        public decimal Sum { get; set; }

        [Display(Name = "Категория расходов")]
        public int SpendingCategoryId { get; set; }

        [Display(Name = "Категория расходов")]
        public  SpendingCategory SpendingCategory { get; set; }

        [Display(Name = "Подкатегория расходов")]
        public int SubCategoryId { get; set; }

        [Display(Name = "Подкатегория расходов")]
        public  SpendingSubCategory SpendingSubCategory { get; set; }

        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Начало периода")]
        public DateTime PeriodBegin { get; set; }

        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Конец периода")]
        public DateTime PeriodEnd { get; set; }

        [Display(Name = "Планируемая сумма")]
        public decimal SumPlaning { get; set; }
        [Display(Name = "Остаток")]
        public decimal Different { get; set; }


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
