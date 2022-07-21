using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models.Interfaces;



namespace FinanceAnalytic.Models.ViewModels
{
    public class SpendingsAnalyticViewModel: IHasDateBeginEnd, IHasSelectedItemsForSpendings
    {
        [Display(Name = "Расходы по категориям")]
        public List<GroupedList> SpendingsByCategoriesList { get; set; }
        public IEnumerable<PlanedSpendingsForAnalyticModel> PlanedSpendingsAnalyticList { get; set; }

        [Display(Name = "Категория расходов")]
        public IEnumerable<SelectListItem> SpendingCategory { get; set; }

        [Display(Name = "Категория расходов")]
        public int SelectedSpendingCategory { get; set; }

        [Display(Name = "Подкатегория расходов")]
        public SelectList SpendingSubCategory { get; set; }
        public int SelectedSpendingSubCategory { get; set; }

        [Display(Name = "Категория важности")]
        public SelectList ImportanceCategory { get; set; }
        public int SelectedImportanceCategory { get; set; }
        public bool HasAnyImpCategory { get; set; }

        [Required(ErrorMessage = "Необходимо указать начало периода")]
        [Display(Name = "Начало периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodBegin { get; set; }
        [Required(ErrorMessage = "Необходимо указать конец периода")]
        [Display(Name = "Конец периода")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime PeriodEnd { get; set; }

        [Display(Name = "Вид периода ")]
        public SelectList PeriodTypes { get; set; }
        public int SelectedPeriodTypes { get; set; }
        public Spending type { get; set; }
    }
}
