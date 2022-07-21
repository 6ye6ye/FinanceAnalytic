using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models;

namespace FinanceAnalytic.Models.ViewModels
{
    public class ChangePasswordVievModel
    {
        [Required(ErrorMessage = "Необходимо ввести текущий пароль.")]

        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string PasswordOld { get; set; }

        [Required(ErrorMessage = "Необходимо ввести новый пароль.")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string PasswordNew { get; set; }
        [Required(ErrorMessage = "Необходимо повторить новый пароль.")]
        [Compare("PasswordNew", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Повторите новый пароль")]
        public string PasswordNewConfirm { get; set; }
        public ChangePasswordVievModel()
        {

        }
    }
}
