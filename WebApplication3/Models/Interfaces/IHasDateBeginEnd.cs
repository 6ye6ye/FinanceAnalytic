

using System;

namespace FinanceAnalytic.Models
{
    public interface IHasDateBeginEnd
    {
        public DateTime PeriodBegin { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
