using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;

namespace FinanceAnalytic.Models
{
    public class SpendingToExcel : ViewModelBase<Spending>
    {

        public DateTime Date { get; set; }

        public virtual SpendingCategory SpendingCategory  { get; set; }
        public string SpendingCategoryName { get; set; }
        public string SpendingSubCategoryName { get; set; }

        public decimal Sum { get; set; }
        public string ImportanceCategoryName { get; set; }

        public string CurrencyName { get; set; }
        public decimal? SumInCurrency { get; set; }
        public decimal? CurrencyRate { get; set; }





        [Display(Name= "Дополнительные коментарии")]
        public string Comments { get; set; }
    }
}