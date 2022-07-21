using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models
{
    public class IncomeSubCategory: IHasName, IHasUser, IHasId
    {

        [Key]
        [Required]
        [Display(Name= "Подвид доходов")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести наименование.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина наименования должна быть от 2 до 30 символов")]
        [Display(Name= "Наименованиие")]
        public string Name{ get; set; }

        [Required(ErrorMessage = "Необходимо выбрать вид доходов.")]
        [Display(Name= "Категория доходов")]
        public int IncomeCategoryId { get; set; }
        [Display(Name= "Категория доходов")]
        public virtual IncomeCategory IncomeCategory { get; set; }

        [Required]
        [Display(Name= "Пользователь")]
        public int UserId { get; set; }

        [Display(Name= "Пользователь")]
        public virtual User User { get; set; }

        public virtual ICollection<Income> Incomes { get; set; }
        public IncomeSubCategory()
        {
            Incomes = new List<Income>();
        }
    }
}
