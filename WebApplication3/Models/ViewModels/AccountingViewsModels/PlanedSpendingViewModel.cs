using FinanceAnalytic.Controllers.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models.ViewModels
{
    public class PlanedSpendingViewModel: ViewModelBase<PlanedSpending>, IValidatableObject
    {
        public PlanedSpendingViewModel( PlanedSpending model) : base(model)
        {

        }
        public PlanedSpendingViewModel()
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
        public int? SpendingCategoryId
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
        public IEnumerable<SelectListItem> ImportanceCategoriesId { get; set; }

        [Display(Name = "Подкатегория расходов")]
        public int? CurrencyId
        {
            get => Model.CurrencyId;
            set => Model.CurrencyId = value;
        }
        [Display(Name = "Валюта")]
        public IEnumerable<SelectListItem> Currencies { get; set; }

        public string UserCurrencyName { get; set; }

        [Required(ErrorMessage = "Необходимо указать начало периода.")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Начало периода")]
        public DateTime PeriodBegin
        {
            get => Model.PeriodBegin;
            set => Model.PeriodBegin = value;
        }

        [Required(ErrorMessage = "Необходимо указать конец периода.")]
        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Конец периода")]
        public DateTime PeriodEnd
        {
            get => Model.PeriodEnd;
            set => Model.PeriodEnd = value;
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
        [Range(0.01, 1000, ErrorMessage = "Значение быть от 0.01 до 1 000")]
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
        // [Display(Name= "Количество")]
        // public int? Count { get; set; }

        // [Display(Name= "Единица измерения")]
        // public int? UnitId { get; set; }
        // [Display(Name= "Единица измерения")]
        //public virtual Unit Unit  { get; set; }

    }
}
