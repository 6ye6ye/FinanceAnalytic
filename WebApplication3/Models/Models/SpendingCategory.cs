using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models
{
    public class SpendingCategory : IHasName, IHasId
    {

        [Key]
        [Required]
        [Display(Name = "Категория расходов")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Необходимо указать наименование.")]
        [Display(Name = "Наименование")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина наименования должна быть от 2 до 30 символов")]
        public string Name { get; set; }
        public virtual ICollection<Spending> Spendings { get; set; }
       // public virtual ICollection<SpendingSubCategory> SpendingSubCategory { get; set; }

    
    public SpendingCategory()
    {
       // spendings = new List<Spendings>();
      //  SpendingSubCategory = new List<SpendingSubCategory>();
    }
    }
}