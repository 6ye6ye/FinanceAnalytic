using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models.ViewModels
{
  
        public class CompareSpendingsAndIncomes
        {
            public DateTime Date { get; set; }
            public string DateString { get; set; }
            public decimal SpendingsSum { get; set; }
            public decimal IncomeSum { get; set; }


    }
   
}
