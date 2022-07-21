using FinanceAnalytic.Controllers.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models
{
    public class PlanedSpending :  IHasId, IHasCurrencyToConvert
    {

        [Key]
        [Required]
        [Display(Name= "Планируемые расходы")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public int UserId { get; set; }

        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать дату.")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Начало периода")]
        public DateTime PeriodBegin { get; set; }

        [Required]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Конец периода")]
        public DateTime PeriodEnd { get; set; }

        [Required]
        [Display(Name= "Планируемые расходы")]
        [RegularExpression(@"^\s*(?=.*[1-9])\d*(?:\.\d{1,2})?\s*$",  ErrorMessage ="Некоректный ввод суммы (пример: 10,55)")]
        public decimal Sum { get; set; }

        [Display(Name = "Валюта конвертации")]
        public int? CurrencyId { get; set; }

        [Display(Name = "Валюта конвертации")]
        public virtual Currency Currency { get; set; }

        [Display(Name = "Сумма в валюте")]
        [Range(1, 1000000000, ErrorMessage = "Сумма должна быть больше от 0.1 до 1000000000")]
        public decimal? SumInCurrency { get; set; }

        [Display(Name = "Значение курса")]
        public decimal? CurrencyRate { get; set; }

        [Display(Name= "Подкатегория расходов")]
        public int? SpendingSubCategoryId { get; set; }

        [Display(Name= "Подкатегория расходов")]
        public virtual SpendingSubCategory SpendingSubCategory { get; set; }

        [Display(Name= "Категория расходов")]
        public int? SpendingCategoryId { get; set; }

        [Display(Name= "Категория расходов")]
        public virtual SpendingCategory SpendingCategory { get; set; }

        [Display(Name = "Дополнительные коментарии")]
        public string Comments { get; set; }

    }
}
