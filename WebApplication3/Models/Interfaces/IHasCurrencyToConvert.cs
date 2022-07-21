using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models
{
    public interface IHasCurrencyToConvert
    {
       
       public int? CurrencyId { get; set; }
       public decimal? SumInCurrency { get; set; }
       public decimal? CurrencyRate { get; set; }
       public decimal Sum { get; set; }
    }
}
