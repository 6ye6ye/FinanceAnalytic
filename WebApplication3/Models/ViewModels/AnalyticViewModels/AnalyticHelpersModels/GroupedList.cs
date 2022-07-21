using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models.ViewModels
{
    public class GroupedList : IHasDate, IHasCategory, IHasSum
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public decimal Sum { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }

    public class UserCountList 
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public decimal UserCount { get; set; }


    }
}
