using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using FinanceAnalytic.Models;
using FinanceAnalytic.Services;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    public class CurrenciesService : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CurrenciesService(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        private bool CurrencyExists(int id)
        {
            return _context.Currencies.Any(e => e.Id == id);
        }

        public ActionResult GetCurrencyRateById(int id)
        {

            var currentCurrency = _context.Currencies.Where(c => c.Id == id).FirstOrDefault();
            if (currentCurrency == null)
            {
                return Json(null);
            }
            var usersCurrency = _context.Users.Include(c => c.Currency).Where(c => c.Login == User.Identity.Name).Select(c => c.Currency).FirstOrDefault();

            if (!_memoryCache.TryGetValue("key_currency", out CurrenciesRatesConverter model))
            {
                throw new Exception("Ошибка получения данных");
            }
        
            switch (usersCurrency.Name)
            {
                case "KGS":
                    switch (currentCurrency.Name)
                    {
                        case "USD":
                            return Json(model.KGS.USD.ToString());
                        case "RUB":
                            return Json(model.KGS.RUB.ToString());
                        case "KZT":
                            return Json(model.KGS.KZT.ToString());
                        case "EUR":
                            return Json(model.KGS.EUR.ToString());
                        default: return Json(1);
                    }
                case "USD":
                    switch (currentCurrency.Name) 
                    {
                        case "KGS":
                            return Json(model.USD.KGS.ToString());
                        case "RUB":
                            return Json(model.USD.RUB.ToString());
                        case "KZT":
                            return Json(model.USD.KZT.ToString());
                        case "EUR":
                            return Json(model.USD.EUR.ToString());
                        default: return Json(1);
                    }
                case "RUB":
                    switch (currentCurrency.Name)
                    {
                        case "KGS":
                            return Json(model.RUB.KGS.ToString());
                        case "USD":
                            return Json(model.RUB.USD.ToString());
                        case "KZT":
                            return Json(model.RUB.KZT.ToString());
                        case "EUR":
                            return Json(model.RUB.EUR.ToString());
                        default: return Json(1);
                    }

                case "EUR":
                    switch (currentCurrency.Name)
                    {
                        case "KGS":
                            return Json(model.EUR.KGS.ToString());
                        case "USD":
                            return Json(model.EUR.USD.ToString());
                        case "KZT":
                            return Json(model.EUR.KZT.ToString());
                        case "RUB":
                            return Json(model.EUR.RUB.ToString());
                        default: return Json(1);
                    }
                case "KZT":
                    switch (currentCurrency.Name)
                    {
                        case "KGS":
                            return Json(model.KZT.KGS.ToString());
                        case "USD":
                            return Json(model.KZT.USD.ToString());
                        case "EUR":
                            return Json(model.KZT.EUR.ToString());
                        case "RUB":
                            return Json(model.KZT.RUB.ToString());
                        default: return Json(1);
                    }
                default: return null;
            }
   
        }
    }
}
