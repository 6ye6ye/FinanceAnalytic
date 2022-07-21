

using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.Enums;
using FinanceAnalytic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static FinanceAnalytic.Models.ViewModels.AllUsersStatistics;

namespace FinanceAnalytic.Controllers
{
    [Authorize(Roles = "admin")]
    public class AnalyticHelperController : Controller
    {
          private readonly AppDbContext _context;


        public AnalyticHelperController(AppDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> AllUsersStatistics(AllUsersStatistics viewModel)
        {
            if (viewModel.PeriodBegin == DateTime.MinValue && viewModel.PeriodEnd == DateTime.MinValue)
            {
                viewModel.SelectedPeriodTypes = 1;
                viewModel.PeriodBegin = DateTime.Now.Date.AddMonths(-1);
                viewModel.PeriodEnd = DateTime.Now.Date;
            }
       
            var usersList = await AnalyticHelper.GetUsersCountListByPeriodType(viewModel.SelectedPeriodTypes, viewModel.PeriodBegin, viewModel.PeriodEnd, _context);
            //var spendingsCountList = await AnalyticHelper.GetCountListByPeriodType(viewModel.SelectedPeriodTypes, viewModel.PeriodBegin, viewModel.PeriodEnd, _context, new Spending());
            ////var spendingsNameList = await AnalyticHelper.GetCountListByPeriodType(viewModel.SelectedPeriodTypes, viewModel.PeriodBegin, viewModel.PeriodEnd, _context, new Spending());
            //var AccumulationsCount = await AnalyticHelper.GetCountListByPeriodType(viewModel.SelectedPeriodTypes, viewModel.PeriodBegin, viewModel.PeriodEnd, _context, new Accumulation());

            var enumData = Enum.GetValues(typeof(PeriodTypesEnum))
                                    .Cast<PeriodTypesEnum>()
                                    .ToList()
                                    .Select(c=>new { Id = ((int)c), Name= c.ToString() });

            viewModel.PeriodTypes = new SelectList(enumData, "Id", "Name");

            //var enumData = from PeriodTypesEnum e in Enum.GetValues(typeof(PeriodTypesEnum))
            //               select new
            //               {
            //                   Id = (int)e,
            //                   Name = e.ToString()
            //               };
            //viewModel.PeriodTypes = new SelectList(enumData.ToList(), "Id", "Name",enumData.);
            //viewModel.PeriodTypes.
            viewModel.UsersRegisterAnalytics = usersList;
            //viewModel.SpendingsByCategories = spendingsCountList;
            /*       viewModel.AccumulationCountByDate= AccumulationsCount*/

            return View(viewModel);
        }
    }








}




