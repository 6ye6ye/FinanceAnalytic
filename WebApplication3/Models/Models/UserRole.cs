using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models
{
    public class UserRole : IHasName, IHasId
    {
        [Key]
        [Required]
        [Display(Name= "Код")]
        public int Id { get; set; }
       
        [Required]
        [StringLength(30, ErrorMessage = "Длина наименования должна быть  до 30 символов")]
        [Display(Name= "Наименование")]
        public string Name{ get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
}