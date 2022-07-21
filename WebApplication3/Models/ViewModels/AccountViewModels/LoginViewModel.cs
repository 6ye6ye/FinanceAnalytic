using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models;

namespace FinanceAnalytic.Models.ViewModels
{
    public class LoginViewModel : ViewModelBase<User>
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
