using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Models;

namespace FinanceAnalytic.Models.ViewModels
{
    public class UserListViewModel
    {
      
        public IEnumerable<User> Users { get; set; }
        [Display(Name= "Роль")]
        public SelectList UserRoles { get; set; }
        [Display(Name= "Логин")]
        public string Login { get; set; }
    }
}
