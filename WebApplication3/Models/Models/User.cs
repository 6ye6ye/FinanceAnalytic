using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FinanceAnalytic.Models
{
    public class User : IHasId, IHasLogin
    {
        [Key]
        [Required]
        [Display(Name= "Пользователь")]
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Необходимо ввести логин.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Длина логина должна быть от 5 до 30 символов")]
        [Display(Name= "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage ="Необходимо ввести пароль.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Длина пароля должна быть от 5 до 30 символов")]
        [Display(Name= "Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Дата регистрации")]
        [BindProperty, DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [Display(Name= "Роль")]
        public int UserRoleId { get; set; }
        [Display(Name= "Роль")]
        public  UserRole UserRole { get; set; }

        [Required]
        [Display(Name = "Валюта")]
        public int CurrencyId { get; set; }

        [Required]
        [Display(Name = "Валюта")]
        public Currency Currency { get; set; }

        public virtual ICollection<SpendingSubCategory> SpendingSubCategory { get; set; }
        public virtual ICollection<Spending> Spendings { get; set; }
        public User()
        {
            SpendingSubCategory = new List<SpendingSubCategory>();
            Spendings = new List<Spending>();
        }
    }
}
