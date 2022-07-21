using NUnit.Framework;
using FinanceAnalytic.Models.ViewModels;
using FinanceAnalytic.Services;

namespace TestProject1
{
    public class CurrencyRateTests
    {

        public CurrenciesRatesConverter currencyConverter ;
        public CurrencyRateTests()
        {
            var actualKGS = new CurrenciesRate()
            {
                KGS = 1,
                KZT = (decimal)0.20,
                RUB = (decimal)1.20,
                EUR = (decimal)85.50,
                USD = (decimal)79.50,
            };
            currencyConverter = new CurrenciesRatesConverter(actualKGS);
        }
        [SetUp]
        public void Setup()
        {
       

        }

        [Test]
        public void CheckGroupByDateType()
        {
       
        }



        [Test]
        public void CurrencyRateForKGSinUSD()
        {
            Assert.AreEqual(0.013, currencyConverter.USD.KGS);    
        }

        [Test]
        public void CurrencyRateForIEqualCurrency()
        {
            Assert.AreEqual(1, currencyConverter.USD.USD);
        }
        [Test]
        public void CurrencyRateForIDifferentCurrensies()
        {
            Assert.AreEqual(0.93, currencyConverter.EUR.USD);
        }

    }
}