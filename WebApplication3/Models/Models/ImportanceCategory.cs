using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models
{
    public class ImportanceCategory : IHasName, IHasId
    {

        [Key]
        [Required]
        [Display(Name= "Дата")]
        public int Id { get; set; }
        [Required]
        [Display(Name= "Наименование")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина наименования должна быть от 2 до 30 символов")]
        public string Name{ get; set; }
        [Required]
        [Display(Name= "Числовой эквивалент")]
        public int NumericEquivalent { get; set; }
        public virtual ICollection<Spending> Spendings { get; set; }

    
    }
}
