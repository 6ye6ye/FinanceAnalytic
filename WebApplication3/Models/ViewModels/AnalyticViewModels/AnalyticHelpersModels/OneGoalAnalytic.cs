using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Models.ViewModels.AnalyticHelpersModels
{
    public class OneGoalAnalytic
    {
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public decimal CurrentSum { get; set; }
        public decimal AccumulationSum { get; set; }
    }
}
