using System;
using System.ComponentModel.DataAnnotations;

using FinanceAnalytic.Models;

namespace FinanceAnalytic.Models.ViewModels
{
    public class UserViewModel : ViewModelBase<User>
    {
        public UserViewModel(User model) : base(model)
        {
        }

        public UserViewModel()
        {
        }
        [Required]
        [Display(Name= "Имя (логин)")]
        public string Name
        {
            get => Model.Login;
            set => Model.Login = value;
        }

        public int Id
        {
            get => Model.Id;
            set => Model.Id = value;
        }

        public int RoleId
        {
            get => Model.UserRoleId;
            set => Model.UserRoleId = value;
        }


        [Display(Name= "Дата регистрации")]
        public DateTime LastAuthDate = DateTime.Now;

     
    }
}
