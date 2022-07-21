using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models.Interfaces
{
    public interface IHasSelectedItemsForSpendings
    {
        public int SelectedSpendingSubCategory { get; set; }
        public int SelectedSpendingCategory { get; set; }
        public int  SelectedImportanceCategory { get; set; }

    }
}
