using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models
{
    public class SpendingSubCategory : IHasName, IHasId, IHasUser
    {
        [Key]
        [Required]
        [Display(Name= "Подкатегория расходов")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо ввести наименование.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина наименования должна быть от 2 до 30 символов")]
        [Display(Name= "Наименованиие")]
        public string Name{ get; set; }

        [Required(ErrorMessage = "Необходимо выбрать категорию.")]
        [Display(Name= "Категория расходов")]
        public int SpendingCategoryId { get; set; }

        [Display(Name= "Категория расходов")]
        public virtual SpendingCategory SpendingCategory { get; set; }
      //  public virtual ICollection<SpendingCategory> spendingCategorieList { get; set; }
        [Required]
        [Display(Name= "Пользователь")]
        public int UserId { get; set; }

        [Display(Name= "Пользователь")]
        public virtual User User { get; set; }

        //public virtual ICollection<Spending> Spendings { get; set; }
        //public virtual ICollection<User> SpendingSubCategories { get; set; }
        //public SpendingSubCategory()
        //{
        //   // spendingCategorieList = new List<SpendingCategory>();
        //    Spendings = new List<Spending>();
        //}
    }
}