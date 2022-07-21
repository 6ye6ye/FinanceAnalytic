using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace FinanceAnalytic.Models.ViewModels
{
    public class GeneralAnalyticViewModel:IHasDateBeginEnd,IValidatableObject
    {
        //-----------------------For spendings------------------
        [Display(Name = "Расходы по категориям")]
        public IEnumerable<CompareSpendingsAndIncomes> CompareList { get; set; }
       
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
