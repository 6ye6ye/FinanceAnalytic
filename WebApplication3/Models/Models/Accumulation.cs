using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models.Models;

namespace FinanceAnalytic.Models
{
    public class Accumulation : IHasId,  IHasDate, IHasUser, IHasSum,IHasCurrencyToConvert
    {
        [Key]
        [Required]
        [Display(Name = "Накопления")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Код пользователя")]
        public int UserId { get; set; }
        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать дату.")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
     
        [Display(Name = "Цель накоплений")]
        public int GoalId { get; set; }
        [Display(Name = "Цель накоплений")]
        public virtual Goal Goal { get; set; }

        [Required(ErrorMessage = "Необходимо указать сумму.")]
        [Display(Name = "Сумма")]
        [Range(0.1, 1000000000, ErrorMessage = "Сумма должна быть больше от 0.1 до 1.000.000.000")]
        public decimal Sum { get; set; }

        [Display(Name = "Валюта конвертации")]
        public int? CurrencyId { get; set; }

        [Display(Name = "Валюта конвертации")]
        public virtual Currency Currency { get; set; }

        [Display(Name = "Сумма в валюте")]
        public decimal? SumInCurrency { get; set; }

        [Display(Name = "Значение курса")]
        public decimal? CurrencyRate { get; set; }

        [Display(Name = "Дополнительные коментарии")]
        public string Comments { get; set; }
    }
}