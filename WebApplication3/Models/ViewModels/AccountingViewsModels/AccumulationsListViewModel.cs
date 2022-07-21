using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FinanceAnalytic.Models.ViewModels
{
    public class AccumulationsListViewModel:IValidatableObject
    {
        public IEnumerable<Accumulation> Accumulations { get; set; }

        public Accumulation AccumulationsParametr { get; set; }

        [Display(Name = "Цель")]
        public SelectList Goals { get; set; }
        public int SelectedGoal { get; set; }
        public bool HideAchived { get; set; } = true;
        [Display(Name = "Начало периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodBegin { get; set; }
        [Display(Name = "Конец периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }
        [Display(Name = "Дата")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Сумма")]
        [BindProperty, DataType(DataType.Currency)]
        public decimal Sum { get; set; }

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
