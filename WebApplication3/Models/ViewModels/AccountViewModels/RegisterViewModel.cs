using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using FinanceAnalytic.Models;

namespace FinanceAnalytic.Models.ViewModels
{
    public class RegisterViewModel : ViewModelBase<User>
    {
        [Required]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Длина логина должна быть от 5 до 25 символов")]

        public string Login
        {
            get => Model.Login;
            set => Model.Login = value;
        }
    

        [Required]
        [DataType(DataType.Password)]
        [Display(Name= "Пароль")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Длина пароля должна быть от 5 до 25 символов")]

        public string Password
        {
            get => Model.Password;
            set => Model.Password = value;
        }
        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name= "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
        [Required]
        public int SelectedCurrencyId
        {
            get => Model.CurrencyId ;
            set => Model.CurrencyId = value;
        }
        [Display(Name = "Валюта")]
        public IEnumerable<SelectListItem> Currencies { get; set; }

    }
}