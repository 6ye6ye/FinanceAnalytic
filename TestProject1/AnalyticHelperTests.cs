using NUnit.Framework;
using System;
using System.Collections.Generic;
using FinanceAnalytic.Controllers.Helpers;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.ViewModels;

namespace TestProject1
{
    public class AnalyticHelperTests
    {
        public List<Spending> spendingsList=new List<Spending>();
        public List<Income> incomesList = new List<Income>();
        public AnalyticHelperTests()
        {
            DateTime date = new DateTime(2000, 1, 1);
            for (int i = 0; i < 6; i++)
            {
                spendingsList.Add(new Spending
                {
                    Date = date.AddDays(10 * i % 2),
                    SpendingCategory = new SpendingCategory() { Name = "Категория" + i % 2 },
                    SpendingSubCategory = new SpendingSubCategory() { Name = "ПодКатегория" + i % 2 },
                    Sum = 100 * i
                });
            }
            date = new DateTime(2000,1,1);
            for (int i = 0; i < 6; i++)
            {
                incomesList.Add(new Income
                {
                    Date = date.AddDays(10 * i % 2),
                    IncomeCategory = new IncomeCategory() { Name = "Категория" + i % 2 },
                    IncomeSubCategory = new IncomeSubCategory() { Name = "ПодКатегория" + i % 2 },
                    Sum = 100 * i
                });
            }
        }
        [SetUp]
        public void Setup()
        {
       

        }

        [Test]
        public void CheckGroupByDateType()
        {     
            Assert.AreEqual(1, AnalyticHelper.GroupByDateType(2, spendingsList).Count);
            Assert.IsInstanceOf<List<GroupedList>>(AnalyticHelper.GroupByDateType(2, spendingsList));
        }

        [Test]
        public void ConvertToGroupedListIncomesListByCategories()
        {
            Assert.AreEqual(2, AnalyticHelper.ConvertToGroupedListIncomesListByCategories(incomesList).Count);
            Assert.IsInstanceOf<List<GroupedList>>(AnalyticHelper.ConvertToGroupedListIncomesListByCategories(incomesList));
        }

        [Test]
        public void ConvertToGroupedListSpendingsListByCategories()
        {
            Assert.AreEqual(2, AnalyticHelper.ConvertToGroupedListSpendingsListByCategories(spendingsList).Count);
            Assert.IsInstanceOf<List<GroupedList>>(AnalyticHelper.ConvertToGroupedListIncomesListByCategories(incomesList));
        }
    }
}