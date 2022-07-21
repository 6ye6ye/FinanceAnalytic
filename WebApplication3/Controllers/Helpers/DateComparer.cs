//using FinanceAnalytic.Models;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Threading.Tasks;

//namespace FinanceAnalytic.Controllers.Helpers
//{

//    public interface DateComparer :IHasDateBeginEnd, IValidatableObject
//    {
  

//        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//        {
//            if (PeriodEnd < PeriodBegin)
//            {
//                yield return new ValidationResult(
//                    errorMessage: "EndDate must be greater than StartDate",
//                    memberNames: new[] { "EndDate" }
//               );
//            }
//        }
//    }
//}
