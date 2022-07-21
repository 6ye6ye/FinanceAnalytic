using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models.ViewModels
{
    public class SpendingsViewModel: ViewModelBase<Spending>
    {
        public SpendingsViewModel(Spending model) : base(model)
        {

        }
        public SpendingsViewModel()
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

        public IEnumerable<Spending> Spendings { get; set; }

        [Display(Name = "Категория расходов")]
        public int SpendingCategoryId
        {
            get => Model.SpendingCategoryId;
            set => Model.SpendingCategoryId = value;
        }
        [Display(Name = "Категория расходов")]
        public IEnumerable<SelectListItem> SpendingCategories { get; set; }

        [Display(Name = "Подкатегория расходов")]
        public int? SpendingSubCategoryId
        {
            get => Model.SpendingSubCategoryId;
            set => Model.SpendingSubCategoryId = value;
        }
        [Display(Name = "Подкатегория расходов")]
        public IEnumerable<SelectListItem> SpendingSubCategories { get; set; }


        [Display(Name = "Категория важности")]
        public int? ImportanceCategoryId
        {
            get => Model.ImportanceCategoryId;
            set => Model.ImportanceCategoryId = value;
        }
        [Display(Name = "Категория важности")]
        public IEnumerable<SelectListItem> ImportanceCategoriesId { get; set; }

        [Display(Name = "Валюта конвертации")]
        public int? CurrencyId
        {
            get => Model.CurrencyId;
            set => Model.CurrencyId = value;
        }
        [Display(Name = "Валюта")]
        public IEnumerable<SelectListItem> Currencies { get; set; }
       public string UserCurrencyName { get; set; }

        [Required(ErrorMessage = "Необходимо выбрать дату.")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date {
            get => Model.Date;
            set => Model.Date = value;
        }
        [Required(ErrorMessage = "Необходимо указать сумму.")]
        [Display(Name = "Сумма")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Сумма должна быть от 0.01 до 1 000 000 000")]
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
        [Range(0.01, 1000, ErrorMessage = "Значение курса должно быть от 0.01 до 1000 ")]
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
    }
}
