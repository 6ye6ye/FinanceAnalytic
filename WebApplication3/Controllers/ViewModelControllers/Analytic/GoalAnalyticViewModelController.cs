using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models.Enums;
using FinanceAnalytic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Controllers
{
    [Authorize]
    public class GoalsAnalyticViewModelController : Controller
    {
        private readonly AppDbContext _context;


        public GoalsAnalyticViewModelController(AppDbContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> GoalAnalytic(GoalAnalyticViewModel viewModel)
        {

            var enumData = from PeriodTypesEnum e in Enum.GetValues(typeof(PeriodTypesEnum))
                           select new
                           {
                               Id = (int)e,
                               Name = e.ToString()
                           };
         
            if (viewModel.SelectedGoal == 0)
            {
                viewModel.OneGoalAnalyticList = null;
                viewModel.GoalsAnalyticList = AnalyticHelper.GetGoalsAnalyticListAsync(User.Identity.Name, viewModel.HideAchived, _context);

            }
            else
            {
                viewModel.GoalsAnalyticList = null;
                viewModel.OneGoalAnalyticList = await AnalyticHelper.GetOneGoalListByPeriodTypeAsync(viewModel.SelectedPeriodTypes, viewModel.SelectedGoal, User.Identity.Name, _context);
               var goal= _context.Goals
                    .Where(c => c.Id == viewModel.SelectedGoal)
                    .FirstOrDefault();
                viewModel.GoalSum = goal.Sum; 
                viewModel.GoalName = goal.Name;
                viewModel.IsAchived = goal.IsAchived;
            };
            var    goalsList = _context.Goals
                    .Where(c => c.User.Login == User.Identity.Name)
                    .OrderBy(c => c.Name)
                    .ToList(); 
            viewModel.Goals = new SelectList(goalsList.ToList(), "Id", "Name");
            viewModel.PeriodTypes = new SelectList(enumData.ToList(), "Id", "Name");
            return View(viewModel);
        }
    }
}
