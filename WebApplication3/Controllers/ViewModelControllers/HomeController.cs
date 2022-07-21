using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FinanceAnalytic.Models;
using FinanceAnalytic.Services;

namespace FinanceAnalytic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger,IConfiguration config, AppDbContext context,  IMemoryCache memoryCache)
        {
            _context = context;
            _logger = logger;
            _config = config;
            _memoryCache = memoryCache;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                
                if (_context.Spendings.Where(s => s.User.Login == User.Identity.Name).Count() == 0)
                    ViewBag.HasSpendings = false;    
                else
                    ViewBag.HasSpendings = true;
                User user = _context.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
     
            }
            if (!_memoryCache.TryGetValue("key_currency", out CurrenciesRatesConverter model))
            {
                throw new Exception("Ошибка получения данных");
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
