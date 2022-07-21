using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinanceAnalytic.Models
{
    public class Currency: IHasId
    {
        [Key]
        [Required]
        [Display(Name= "Валюта")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Необходимо указать наименование.")]
        [Display(Name= "Валюта")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Длина наименования должна быть от 2 до 30 символов")]
        public string Name{ get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
