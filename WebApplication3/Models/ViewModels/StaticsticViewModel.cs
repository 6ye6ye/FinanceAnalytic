using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.ViewsModels
{
    public class StaticsticViewModel
    {
        [Display(Name = "Наименование")]
        public IEnumerable<Spending> spending { get; set; }
        
        [Display(Name = "Категория расходов")]
        public SelectList SpendingCategorie { get; set; }
        [Display(Name = "Подкатегория расходов")]
        public SelectList SubCategorie { get; set; }
        [Display(Name = "Начало периода")]
        public DateTime dateBegin { get; set; }
        [Display(Name = "Конец периода")]
        public DateTime dateEnd { get; set; }
        [Display(Name = "Логин")]
        public string Login { get; set; }
    }
}
