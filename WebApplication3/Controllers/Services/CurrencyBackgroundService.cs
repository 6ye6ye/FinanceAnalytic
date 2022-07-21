using FinanceAnalytic.Models.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinanceAnalytic.Services
{
    public class CurrencyBackgroundService : BackgroundService
    {
        private readonly IMemoryCache memoryCache;

        public CurrencyBackgroundService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU"); 
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    XDocument xml = XDocument.Load("https://www.nbkr.kg/XML/daily.xml");
                   
                    var actualKGS = new CurrenciesRate()
                    {
                        KGS = 1,
                        KZT = Math.Round(Convert.ToDecimal(xml.Elements("CurrencyRates").Elements("Currency").Where(x => x.Attribute("ISOCode").Value == "KZT").Select(x => x.Element("Value").Value).FirstOrDefault()), 2),
                        RUB = Math.Round(Convert.ToDecimal(xml.Elements("CurrencyRates").Elements("Currency").Where(x => x.Attribute("ISOCode").Value == "RUB").Select(x => x.Element("Value").Value).FirstOrDefault()), 2),
                        EUR = Math.Round(Convert.ToDecimal(xml.Elements("CurrencyRates").Elements("Currency").Where(x => x.Attribute("ISOCode").Value == "EUR").Select(x => x.Element("Value").Value).FirstOrDefault()), 2),
                        USD = Math.Round(Convert.ToDecimal(xml.Elements("CurrencyRates").Elements("Currency").Where(x => x.Attribute("ISOCode").Value == "USD").Select(x => x.Element("Value").Value).FirstOrDefault()), 2)
                    };
                    CurrenciesRatesConverter currencyConverter = new CurrenciesRatesConverter(actualKGS);
                    memoryCache.Set("key_currency", currencyConverter, TimeSpan.FromMinutes(1440));
                }
                catch 
                {
                }
                await Task.Delay(TimeSpan.FromHours(2), stoppingToken);
            }
        }
    }
}
