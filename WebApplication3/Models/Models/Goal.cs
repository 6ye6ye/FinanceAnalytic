using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models.Models
{
    public class Goal:IHasName, IHasUser, IHasId
    {
        [Key]
        [Required]
        [Display(Name = "Цель")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо указать наименование.")]
        [Display(Name = "Наименование")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина наименования должна быть от 2 до 30 символов")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Необходимая сумма")]
        [Range(0.1, 1000000000000, ErrorMessage = "Сумма должна быть больше от 0.1 до 1.000.000.000.000")]
        public decimal Sum { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public int UserId { get; set; }

        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }

        [Display(Name = "Цель достигнута")]
        public bool IsAchived { get; set; }     
    }
}
