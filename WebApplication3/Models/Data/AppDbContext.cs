using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinanceAnalytic.Models;
using FinanceAnalytic.Models.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        string adminRoleName = "admin";
        string userRoleName = "User";
        UserRole adminRole = new UserRole { Id=1, Name= adminRoleName };
        UserRole userRole = new UserRole { Id=2, Name= userRoleName };
        //string adminLogin = "admin";
        //string adminPassword= "admin123";
        User userAdmin = new User
        {
            Id=1,
            Login = (string)"admin",
            Password = (string)"admin123",
            CurrencyId=1,
            UserRoleId = adminRole.Id
        };
        SpendingCategory сategorie1 = new SpendingCategory { Id = 1, Name = (string)"Другое" };
        SpendingCategory сategorie2 = new SpendingCategory { Id = 2, Name = (string)"Транспорт" };
        SpendingCategory сategorie3 = new SpendingCategory { Id = 3, Name = (string)"Оплата" };
        SpendingCategory сategorie4 = new SpendingCategory { Id = 4, Name = (string)"Покупки" };
        SpendingCategory сategorie5 = new SpendingCategory { Id = 5, Name = (string)"Продукты" };
        SpendingCategory сategorie6 = new SpendingCategory { Id = 6, Name = (string)"Питание вне дома" };
        SpendingCategory сategorie7 = new SpendingCategory { Id = 7, Name = (string)"Досуг"};
        SpendingCategory сategorie8 = new SpendingCategory { Id = 8, Name = (string)"На здоровье" };

        IncomeCategory IncomeCategory1 = new IncomeCategory { Id = 1, Name = (string)"Другие источники" };
        IncomeCategory IncomeCategory2 = new IncomeCategory { Id = 2, Name = (string)"Зарплата" };
        IncomeCategory IncomeCategory4 = new IncomeCategory { Id = 4, Name = (string)"Гос. выплаты" };
        IncomeCategory IncomeCategory5 = new IncomeCategory { Id = 5, Name = (string)"Сдача в аренду" };
        IncomeCategory IncomeCategory6 = new IncomeCategory { Id = 6, Name = (string)"Предпринимательство" };
        IncomeCategory IncomeCategory7 = new IncomeCategory { Id = 7, Name = (string)"Подсобное хозяйство" };

        ImportanceCategory ImportanceCategory1 = new ImportanceCategory { Id = 1, Name = (string)"Не важно", NumericEquivalent=0 };
        ImportanceCategory ImportanceCategory2 = new ImportanceCategory { Id = 2, Name = (string)"Можно обойтись",NumericEquivalent=1 };
        ImportanceCategory ImportanceCategory3 = new ImportanceCategory { Id = 3, Name = (string)"Желательно", NumericEquivalent = 2 };
        ImportanceCategory ImportanceCategory4 = new ImportanceCategory { Id = 4, Name = (string)"Необходимо", NumericEquivalent = 3 };
        ImportanceCategory ImportanceCategory5 = new ImportanceCategory { Id = 5, Name = (string)"Крайне необходимо", NumericEquivalent = 4 };
       
        Currency Currency1 = new Currency { Id = 1, Name = (string)"KGS" };
        Currency Currency2 = new Currency { Id = 2, Name = (string)"USD"};
        Currency Currency3 = new Currency { Id = 3, Name = (string)"EUR"};
        Currency Currency4 = new Currency { Id = 4, Name = (string)"RUB"};
        Currency Currency5 = new Currency { Id = 5, Name = (string)"KZT"};
       

        modelBuilder.Entity<UserRole>().HasData(new UserRole[] { adminRole, userRole });
        modelBuilder.Entity<User>().HasData(new User[] { userAdmin });
        modelBuilder.Entity<SpendingCategory>().HasData(new SpendingCategory[] { сategorie1, сategorie2, сategorie3, сategorie4, сategorie5, сategorie6, сategorie7, сategorie8 });
        modelBuilder.Entity<IncomeCategory>().HasData(new IncomeCategory[] { IncomeCategory1, IncomeCategory2, IncomeCategory4, IncomeCategory5, IncomeCategory6, IncomeCategory7 });
        modelBuilder.Entity<ImportanceCategory>().HasData(new ImportanceCategory[] { ImportanceCategory1, ImportanceCategory2, ImportanceCategory3, ImportanceCategory4, ImportanceCategory5});
        modelBuilder.Entity<Currency>().HasData(new Currency[] { Currency1, Currency2, Currency3, Currency4, Currency5 });
    }



    public DbSet<User> Users { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<IncomeSubCategory> IncomeSubCategories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<PlanedSpending> PlanedSpendings { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<SpendingCategory> SpendingCategories { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<SpendingSubCategory> SpendingSubCategories { get; set; }

        public DbSet<ImportanceCategory> ImportanceCategories { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Accumulation> Accumulations { get; set; }

    
}
