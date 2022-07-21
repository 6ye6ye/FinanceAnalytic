using ClosedXML.Excel;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Models.ViewModels.AnalyticHelpersModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FinanceAnalytic.Controllers.Helpers
{
    public class ConverterToExcelHelper : Controller
    {
        private readonly AppDbContext _context;
        public ConverterToExcelHelper()
        {
        }

        public void FillByGroupedList(XLWorkbook workbook, string typeName, string header, List<GroupedList> groupedList)
        {
            var worksheet = workbook.Worksheets.Add(typeName);
            var wsReportNameHeaderRange = worksheet.Range("A1:D1");
            wsReportNameHeaderRange.Style.Font.Bold = true;
            wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            wsReportNameHeaderRange.Merge();
            wsReportNameHeaderRange.Value = header;
            worksheet.Cell("A2").Value = "Дата";
            worksheet.Cell("B2").Value = "Категория";
            worksheet.Cell("C2").Value = "Сумма";
            for (int i = 0; i < groupedList.Count; i++)
            {
                worksheet.Cell(i + 3, 1).Value = groupedList[i].DateString;
                worksheet.Cell(i + 3, 2).Value = groupedList[i].CategoryName;       
                worksheet.Cell(i + 3, 3).Value = groupedList[i].Sum;
            }
            worksheet.Cell(groupedList.Count + 3, 2).Value = "Сумма итого:";
            worksheet.Cell(groupedList.Count + 3, 3).Value = groupedList.Sum(c => c.Sum);
            worksheet.Columns().AdjustToContents();
        }

        [HttpPost]
        public FileContentResult ConvertToExcelSpendingsAnalytic(List<GroupedList> groupedList, string PeriodBegin, string PeriodEnd)
        {

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                FillByGroupedList(workbook, "Расходы", "Расходы за период с " + PeriodBegin + " по " + PeriodEnd, groupedList);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_Spendings_{DateTime.Now.ToShortDateString()}.xlsx"); //+ @DateTime.Now.ToShortDateString() +                                                                                                                                                                                 // };
                }
            }
        }

        [HttpPost]
        public FileContentResult ConvertToExcelIncomesAnalytic(List<GroupedList> groupedList, DateTime PeriodBegin, DateTime PeriodEnd)
        {

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                FillByGroupedList(workbook, "Доходы", "Доходы за период с " + PeriodBegin.ToShortDateString() + " по " + PeriodEnd.ToShortDateString(), groupedList);
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_IncomesA_{DateTime.Now.ToShortDateString()}.xlsx"); //+ @DateTime.Now.ToShortDateString() +                                                                                                                                                                                 // };
                }
            }
        }

        [HttpPost]
        public FileContentResult ConvertToExcelPlanedSpendingsAnalytic(List<PlanedSpendingsForAnalyticModel> groupedList, DateTime PeriodBegin, DateTime PeriodEnd)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Планируемые расходы");
                var wsReportNameHeaderRange = worksheet.Range("A1:D1");
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "Планируемые расходы за период с " + PeriodBegin.ToShortDateString() + " по " + PeriodEnd.ToShortDateString();
                worksheet.Cell("A2").Value = "Начало периода";
                worksheet.Cell("B2").Value = "Конец периода";
                worksheet.Cell("C2").Value = "Категория";
                worksheet.Cell("D2").Value = "Подкатегория";
                worksheet.Cell("E2").Value = "Сумма планируемая";
                worksheet.Cell("F2").Value = "Сумма потраченная";
                for (int i = 0; i < groupedList.Count; i++)
                {
                    worksheet.Cell(i + 3, 1).Value = groupedList[i].PeriodBegin;
                    worksheet.Cell(i + 3, 2).Value = groupedList[i].PeriodEnd;
                    worksheet.Cell(i + 3, 3).Value = groupedList[i].SpendingCategory;
                    worksheet.Cell(i + 3, 4).Value = groupedList[i].SpendingSubCategory;
                    worksheet.Cell(i + 3, 5).Value = groupedList[i].SumPlaning;
                    worksheet.Cell(i + 3, 6).Value = groupedList[i].Sum;
                }
                worksheet.Cell(groupedList.Count + 3, 4).Value = "Сумма итого:";
                worksheet.Cell(groupedList.Count + 3, 5).Value = groupedList.Sum(c => c.SumPlaning);
                worksheet.Cell(groupedList.Count + 3, 6).Value = groupedList.Sum(c => c.Sum);
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_PlanedSpendings_{DateTime.Now.ToShortDateString()}.xlsx");                                                                                                                                                                             // };
                }
            }
        }

        [HttpPost]
        public FileContentResult ConvertToExcelGeneralAnalytic(List<CompareSpendingsAndIncomes> list, DateTime PeriodBegin, DateTime PeriodEnd)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Доходы-расходы");
                var wsReportNameHeaderRange = worksheet.Range("A1:D1");
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "Доходы-расходы за период с " + PeriodBegin.ToShortDateString() + " по " + PeriodEnd.ToShortDateString();
                worksheet.Cell("A2").Value = "Дата";
                worksheet.Cell("B2").Value = "Доходы";
                worksheet.Cell("C2").Value = "Расходы";
                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cell(i + 3, 1).Value = list[i].DateString;
                    worksheet.Cell(i + 3, 2).Value = list[i].IncomeSum;
                    worksheet.Cell(i + 3, 3).Value = list[i].SpendingsSum;
                }
                var incSum = list.Sum(c => c.IncomeSum);
                var spSum = list.Sum(c => c.SpendingsSum);
                worksheet.Cell(list.Count + 3, 1).Value = "Итого:";
                worksheet.Cell(list.Count + 3, 2).Value = incSum;
                worksheet.Cell(list.Count + 3, 3).Value = spSum;
                worksheet.Cell(list.Count + 4, 1).Value = "Чистая прибыль:";
                worksheet.Cell(list.Count + 4, 2).Value = incSum - spSum;
                worksheet.Cell(list.Count + 4, 2).Style.Fill.BackgroundColor = XLColor.Yellow;

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_Incomes_Spendings_{DateTime.Now.ToShortDateString()}.xlsx");                                                                                                                                                                             // };
                }
            }
        }

        [HttpPost]
        public FileContentResult ConvertToExcelGoalAnalytics(List<OneGoalAnalytic> oneGoalList, string? goalName, decimal? goalSum, List<GoalsAnalytic> goalsList)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Накопления");
                var wsReportNameHeaderRange = worksheet.Range("A1:D1");
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                if (oneGoalList.Count() != 0)
                {
                    wsReportNameHeaderRange.Value = DateTime.UtcNow.ToShortDateString() + ". Накопления на " + goalName + ". Цель: " + goalSum;
                    worksheet.Cell("A2").Value = "Дата";
                    worksheet.Cell("B2").Value = "Сумма";
                    worksheet.Cell("C2").Value = "Накопленная сумма";
                    for (int i = 0; i < oneGoalList.Count; i++)
                    {
                        worksheet.Cell(i + 3, 1).Value = oneGoalList[i].Date;
                        worksheet.Cell(i + 3, 2).Value = oneGoalList[i].CurrentSum;
                        worksheet.Cell(i + 3, 3).Value = oneGoalList[i].AccumulationSum;
                    }
                    var different = goalSum - oneGoalList.Last().AccumulationSum;
                    worksheet.Cell(oneGoalList.Count + 3, 2).Value = "Осталось накопить:";
                    worksheet.Cell(oneGoalList.Count + 3, 3).Value = different;
                    worksheet.Cell(oneGoalList.Count + 3, 3).Style.Fill.BackgroundColor = XLColor.Yellow;
                }
                else
                {
                    wsReportNameHeaderRange.Value = DateTime.UtcNow.ToShortDateString() + ". Накопления по целям ";
                    worksheet.Cell("A2").Value = "Дата";
                    worksheet.Cell("B2").Value = "Сумма накопленная";
                    worksheet.Cell("C2").Value = "Сумма необходимая";
                    for (int i = 0; i < goalsList.Count; i++)
                    {
                        worksheet.Cell(i + 3, 1).Value = goalsList[i].GoalName;
                        worksheet.Cell(i + 3, 2).Value = goalsList[i].CurrentSum;
                        worksheet.Cell(i + 3, 3).Value = goalsList[i].GoalSum;
                    }
                }
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_Accumulations_{DateTime.Now.ToShortDateString()}.xlsx");                                                                                                                                                                             // };
                }
            }
        }

        [HttpPost]
        public FileContentResult CompareToExcelSpendingsList(List<SpendingToExcel> list, DateTime PeriodBegin, DateTime PeriodEnd)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Расходы");

                var wsReportNameHeaderRange = worksheet.Range("A1:F1");
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "Расходы за период с " + PeriodBegin.ToShortDateString() + " по " + PeriodEnd.ToShortDateString();
                worksheet.Cell("A2").Value = "Дата";
                worksheet.Cell("B2").Value = "Сумма";
                worksheet.Cell("C2").Value = "Категория";
                worksheet.Cell("D2").Value = "Подкатегория";
                worksheet.Cell("E2").Value = "Важность";
                worksheet.Cell("F2").Value = "Валюта";
                worksheet.Cell("G2").Value = "Курс валюты";
                worksheet.Cell("H2").Value = "Сумма в валюте";
                worksheet.Cell("I2").Value = "Коментарии";
                int i = 0;
                foreach (var item in list)
                {
                    worksheet.Cell(i + 3, 1).Value = item.Date;
                    worksheet.Cell(i + 3, 2).Value = item.Sum;
                    worksheet.Cell(i + 3, 3).Value = item.SpendingCategoryName;
                    worksheet.Cell(i + 3, 4).Value = item.SpendingSubCategoryName;
                    worksheet.Cell(i + 3, 5).Value = item.ImportanceCategoryName;
                    worksheet.Cell(i + 3, 6).Value = item.CurrencyName;
                    worksheet.Cell(i + 3, 7).Value = item.CurrencyRate;
                    worksheet.Cell(i + 3, 8).Value = item.SumInCurrency;
                    worksheet.Cell(i + 3, 9).Value = item.Comments;
                    i++;
                }
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_Spendings_{DateTime.Now.ToShortDateString()}.xlsx"); //+ @DateTime.Now.ToShortDateString() +                                                                                                                                                                                 // };
                }
            }
        }

        [HttpPost]
        public FileContentResult CompareToExcelPlanedSpendingsList(IEnumerable<PlanedSpending> list, DateTime PeriodBegin, DateTime PeriodEnd)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Планируемые расходы");
                var wsReportNameHeaderRange = worksheet.Range("A1:F1");
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "Планируемые расходы за период с " + PeriodBegin.ToShortDateString() + " по " + PeriodEnd.ToShortDateString();
                worksheet.Cell("A2").Value = "Начало периода";
                worksheet.Cell("B2").Value = "Конец периода";
                worksheet.Cell("C2").Value = "Сумма";
                worksheet.Cell("D2").Value = "Категория";
                worksheet.Cell("E2").Value = "Подкатегория";
                worksheet.Cell("F2").Value = "Валюта";
                worksheet.Cell("G2").Value = "Курс валюты";
                worksheet.Cell("H2").Value = "Сумма в валюте";
                int i = 0;
                foreach (var item in list)
                {
                    worksheet.Cell(i + 3, 1).Value = item.PeriodBegin;
                    worksheet.Cell(i + 3, 2).Value = item.PeriodEnd;
                    worksheet.Cell(i + 3, 3).Value = item.Sum;
                    worksheet.Cell(i + 3, 4).Value = item.SpendingCategory;
                    worksheet.Cell(i + 3, 5).Value = item.SpendingSubCategory;
                    worksheet.Cell(i + 3, 6).Value = item.Currency;
                    worksheet.Cell(i + 3, 7).Value = item.CurrencyRate;
                    worksheet.Cell(i + 3, 8).Value = item.SumInCurrency;
                }
                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_PlanedSpendings_{DateTime.Now.ToShortDateString()}.xlsx"); //+ @DateTime.Now.ToShortDateString() +                                                                                                                                                                                 // };
                }
            }
        }

        [HttpPost]
        public FileContentResult CompareToExcelIncomesList(IEnumerable<Income> list, DateTime PeriodBegin, DateTime PeriodEnd)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Доходы");
                var wsReportNameHeaderRange = worksheet.Range("A1:C1");
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "Доходы за период с " + PeriodBegin.ToShortDateString() + " по " + PeriodEnd.ToShortDateString();
                worksheet.Cell("A2").Value = "Дата";
                worksheet.Cell("B2").Value = "Сумма";
                worksheet.Cell("C2").Value = "Категория";
                worksheet.Cell("D2").Value = "Подкатегория";
                worksheet.Cell("E2").Value = "Валюта";
                worksheet.Cell("F2").Value = "Курс валюты";
                worksheet.Cell("G2").Value = "Сумма в валюте";
                worksheet.Cell("H2").Value = "Коментарии";
                int i = 0;
                foreach (var item in list)
                {

                    worksheet.Cell(i + 3, 1).Value = item.Date;
                    worksheet.Cell(i + 3, 2).Value = item.Sum;
                    if (item.IncomeCategory != null) worksheet.Cell(i + 3, 3).Value = item.IncomeCategory.Name;
                    if (item.IncomeSubCategory != null) worksheet.Cell(i + 3, 4).Value = item.IncomeSubCategory.Name;
                    if (item.Currency != null) worksheet.Cell(i + 3, 5).Value = item.Currency.Name;
                    if (item.CurrencyRate != null) worksheet.Cell(i + 3, 6).Value = item.CurrencyRate;
                    if (item.SumInCurrency != null) worksheet.Cell(i + 3, 7).Value = item.SumInCurrency;
                    worksheet.Cell(i + 3, 8).Value = item.Comments;
                    i++;
                }

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_Incomes_{DateTime.Now.ToShortDateString()}.xlsx"); //+ @DateTime.Now.ToShortDateString() +                                                                                                                                                                                 // };
                }
            }
        }

        [HttpPost]
        public FileContentResult CompareToExcelAccumulationsList(IEnumerable<Accumulation> list, DateTime PeriodBegin, DateTime PeriodEnd)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Накопления");
                var wsReportNameHeaderRange = worksheet.Range("A1:C1");
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = "Накопления за период с " + PeriodBegin.ToShortDateString() + " по " + PeriodEnd.ToShortDateString();
                worksheet.Cell("A2").Value = "Дата";
                worksheet.Cell("B2").Value = "Сумма";
                worksheet.Cell("C2").Value = "Цель";
                worksheet.Cell("D2").Value = "Валюта";
                worksheet.Cell("E2").Value = "Курс валюты";
                worksheet.Cell("F2").Value = "Сумма в валюте";
                worksheet.Cell("G2").Value = "Коментарии";
                int i = 0;
                foreach (var item in list)
                {
                    worksheet.Cell(i + 3, 1).Value = item.Date;
                    worksheet.Cell(i + 3, 2).Value = item.Sum;
                    worksheet.Cell(i + 3, 3).Value = item.Goal.Name;
                    if (item.Currency != null) worksheet.Cell(i + 3, 4).Value = item.Currency.Name;
                    if (item.CurrencyRate != null) worksheet.Cell(i + 3, 5).Value = item.CurrencyRate;
                    if (item.SumInCurrency != null) worksheet.Cell(i + 3, 6).Value = item.SumInCurrency;
                    worksheet.Cell(i + 3, 7).Value = item.Comments;
                    i++;
                }

                worksheet.Columns().AdjustToContents();
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    // stream.Flush();
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{User.Identity.Name}_Accumulation_{DateTime.Now.ToShortDateString()}.xlsx"); //+ @DateTime.Now.ToShortDateString() +                                                                                                                                                                                 // };
                }
            }
        }
    }
}