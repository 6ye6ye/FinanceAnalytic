using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;


namespace FinanceAnalytic.Models
{
    public class Spending : IHasId, IHasCurrencyToConvert, IHasDate,IHasUser, IHasSum
    {
        [Key]
        [Required]
        [Display(Name= "Расходы")]
        public int Id { get; set; }
  
        [Required]
        [Display(Name= "Код пользователя")]
        public int UserId { get; set; }
        [Display(Name= "Пользователь")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать дату.")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name= "Дата")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Необходимо указать категорию.")]
        [Display(Name= "Категория расходов")]
        public int SpendingCategoryId { get; set; }
        [Display(Name= "Категория расходов")]
        public virtual SpendingCategory SpendingCategory  { get; set; }

        [Display(Name= "Подкатегория расходов")]
        public int? SpendingSubCategoryId { get; set; }
        [Display(Name= "Подкатегория расходов")]
        public virtual SpendingSubCategory SpendingSubCategory { get; set; }
        
      
        [Range(0, double.MaxValue, ErrorMessage = "Сумма должна быть больше нуля")]
        [Display(Name= "Сумма")]
        [Required(ErrorMessage = "Необходимо указать сумму.")]
        public decimal Sum { get; set; }

        [Display(Name= "Валюта конвертации")]
        public int? CurrencyId { get; set; }
        [Display(Name= "Валюта конвертации")]
        public virtual Currency Currency { get; set; }

        [Display(Name = "Сумма в валюте")]
        [Range(0.1, 1000000000, ErrorMessage = "Сумма должна быть больше от 0.1 до 1.000.000.000")]
        public decimal? SumInCurrency { get; set; }

        [Display(Name= "Значение курса")]
        public decimal? CurrencyRate { get; set; }

       // [Display(Name= "Количество")]
       // public int? Count { get; set; }

       // [Display(Name= "Единица измерения")]
       // public int? UnitId { get; set; }
       // [Display(Name= "Единица измерения")]
       //public virtual Unit Unit  { get; set; }

        [Display(Name= "Категория важности")]
        public int? ImportanceCategoryId { get; set; }
        [Display(Name= "Категория важности")]
        public virtual ImportanceCategory ImportanceCategory { get; set; }

        [Display(Name= "Дополнительные коментарии")]
        public string Comments { get; set; }
    }
}