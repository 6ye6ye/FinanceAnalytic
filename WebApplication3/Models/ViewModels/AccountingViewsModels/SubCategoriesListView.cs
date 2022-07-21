using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Models;

namespace FinanceAnalytic.Models.ViewModels
{
    public class SpendingSubCategorieListView
    {
        [Display(Name= "Наименование")]
        public IEnumerable<SpendingSubCategory> SpendingSubCategory { get; set; }
        [Display(Name= "Категория расходов")]
        public SelectList SpendingCategories { get; set; }
    }
}
