using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models.ViewModels
{
    public class IncomesViewModel: ViewModelBase<Income>
    {
        public IncomesViewModel(Income model) : base(model)
        {

        }
        public IncomesViewModel()
        {
        }
    
 
        public int Id
        {
            get => Model.Id;
            set => Model.Id = value;
        }
        [Required]
        [Display(Name = "Код пользователя")]
        public int UserId {
            get => Model.UserId;
            set => Model.UserId = value;
        }

        [Display(Name = "Пользователь")]
        public string UserLogin => Model.User?.Login;

        public IEnumerable<Income> Incomes { get; set; }


        [Display(Name = "Категория доходов")]
        public int IncomeCategoryId
        {
            get => Model.IncomeCategoryId;
            set => Model.IncomeCategoryId = value;
        }
        [Display(Name = "Категория доходов")]
        public SelectList IncomeCategories { get; set; }
     
        [Display(Name = "Подкатегория доходов")]
        public int? IncomeSubCategoryId
        {
            get => Model.IncomeSubCategoryId;
            set => Model.IncomeSubCategoryId = value;
        }
        [Display(Name = "Подкатегория доходов")]
        public SelectList IncomeSubCategories { get; set; }
        [Display(Name = "Валюта")]
        public int? CurrencyId
        {
            get => Model.CurrencyId;
            set => Model.CurrencyId = value;
        }
        [Display(Name = "Валюта")]
        public IEnumerable<SelectListItem> Currencies { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать дату.")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date {
            get => Model.Date;
            set => Model.Date = value;
        }
    
        [Required(ErrorMessage = "Необходимо указать сумму.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Сумма должна быть от 0.01 до 1 000 000 000")]
        [Display(Name = "Сумма")]
        public decimal Sum
        {
            get => Model.Sum;
            set => Model.Sum = value;
        }

        [Display(Name = "Сумма в валюте")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Сумма должна быть от 0.01 до 1 000 000 000")]
        public decimal? SumInCurrency
        {
            get => Model.SumInCurrency;
            set => Model.SumInCurrency = value;
        }

        [Display(Name = "Значение курса")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Значение курса должно быть от 0.01 до 1 000")]
        public decimal? CurrencyRate
        {
            get => Model.CurrencyRate;
            set => Model.CurrencyRate = value;
        }
       
        [Display(Name = "Дополнительные коментарии")]
        public string Comments
        {
            get => Model.Comments;
            set => Model.Comments = value;
        }
        public string UserCurrencyName { get; set; }
    }
}
