using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinanceAnalytic.Models.ViewModels.AnalyticHelpersModels;

namespace FinanceAnalytic.Models.ViewModels
{
    public class GoalAnalyticViewModel
    {
        [Display(Name = "Цели")]
        public List<GoalsAnalytic> GoalsAnalyticList { get; set; }
        [Display(Name = "Цели")]
        public List<OneGoalAnalytic> OneGoalAnalyticList { get; set; }
        [Display(Name = "Цель ")]
        public SelectList Goals { get; set; }
        public int SelectedGoal { get; set; }
        public bool HideAchived { get; set; } = true;
        public bool IsAchived { get; set; } 
        public string GoalName { get; set; }
        public decimal GoalSum { get; set; }
        [Display(Name = "Вид периода ")]
        public SelectList PeriodTypes { get; set; }
        public int SelectedPeriodTypes { get; set; }
    }
}
