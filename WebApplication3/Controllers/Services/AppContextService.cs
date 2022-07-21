using FinanceAnalytic.Models;
using FinanceAnalytic.Models.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceAnalytic.Services
{
    public static class AppContextService
    {
        public static async Task AddWithCheckName<T>(this AppDbContext context, T entity) where T : class, IHasName
        {
            if (context.Set<T>().Any(x => x.Name == entity.Name))
                throw new Exception("Данное наименование уже существует");

            context.Add(entity);
            await context.SaveChangesAsync();
        }
        public static async Task AddWithCheckNameForUser<T>(this AppDbContext context, T entity) where T : class, IHasName, IHasUser
        {
            if (context.Set<T>().Any(x => x.Name == entity.Name && x.UserId==entity.UserId))
                throw new Exception("Данное наименование уже существует");

            context.Add(entity);
            await context.SaveChangesAsync();
        }
        public static async Task AddWithCheckLogin<T>(this AppDbContext context, T entity) where T : class, IHasLogin
        {
            if (context.Set<T>().Any(x => x.Login == entity.Login))
                throw new Exception("Пользователь с данным логином уже существует");

            context.Add(entity);
            await context.SaveChangesAsync();
        }

        public static async Task AddWithCheckCurrency<T>(this AppDbContext context, T entity) where T : class, IHasCurrencyToConvert
        {
            if (entity.SumInCurrency != null && entity.CurrencyRate == null && entity.Sum != 0)
            {
                entity.CurrencyRate = entity.Sum / entity.SumInCurrency;
            }
            try
            {
                context.Add(entity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Ошибка ввода данных");
            }
        }

        public static async Task EditwithCheckName<T>(this AppDbContext context, T entity) where T : class, IHasName, IHasId
        {
            if (context.Set<T>().Any(x => x.Name == entity.Name && x.Id != entity.Id))
                throw new Exception("Данное наименование уже существует");

            context.Update(entity);
            await context.SaveChangesAsync();
        }
        public static async Task EditwithCheckNameForUser<T>(this AppDbContext context, T entity) where T : class, IHasName, IHasId,IHasUser
        {
            if (context.Set<T>().Any(x => x.Name == entity.Name && x.Id != entity.Id && x.UserId == entity.UserId))
                throw new Exception("Данное наименование уже существует");

            context.Update(entity);
            await context.SaveChangesAsync();
        }
        
        public static async Task EditwithCheckCurrency<T>(this AppDbContext context, T entity) where T : class, IHasCurrencyToConvert
        {

            if (entity.Sum <= 0 || entity.Sum == 0)
            {
                throw new Exception("Сумма должна быть больше 0");
            }
            if (entity.CurrencyId == null && entity.SumInCurrency != null)
                throw new Exception("При указывании суммы в валюте необходимо указать валюту");
            if (entity.CurrencyId != null && entity.SumInCurrency != null && entity.CurrencyRate == null && entity.Sum != 0)
            {
                entity.CurrencyRate = entity.Sum / entity.SumInCurrency;
            }
            try
            {
                context.Update(entity);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Ошибка ввода данных");
            }
        }
        public static async Task SpendingSubCategoriesDelete(this AppDbContext context, SpendingSubCategory SpendingSubCategory)
        {
            try
            {
                var spendings = context.Spendings.Where(c => c.SpendingSubCategoryId == SpendingSubCategory.Id);
                var PlanedSpendings = context.PlanedSpendings.Where(c => c.SpendingSubCategoryId == SpendingSubCategory.Id);
                foreach (var c in spendings)
                {
                    c.SpendingSubCategory = null;
                    c.SpendingSubCategoryId = null;
                    context.Spendings.Update(c);
                }
                foreach (var c in PlanedSpendings)
                {
                    c.SpendingSubCategory = null;
                    c.SpendingSubCategoryId = null;
                    context.PlanedSpendings.Update(c);
                }
                context.SpendingSubCategories.Remove(SpendingSubCategory);
                await context.SaveChangesAsync();
            }
            catch (Exception )
            {
                throw new Exception("Ошибка. Удаление отменено.");
            }
        }
        public static async Task IncomeSubCategoriesDelete(this AppDbContext context, IncomeSubCategory incomeSubCategory)
        {
            try
            {
                var incomes = context.Incomes.Where(c => c.IncomeSubCategoryId == incomeSubCategory.Id);
                foreach (var c in incomes)
                {
                    c.IncomeSubCategory = null;
                    c.IncomeSubCategoryId = null;
                    context.Incomes.Update(c);
                }
                context.IncomeSubCategories.Remove(incomeSubCategory);
                await context.SaveChangesAsync();
            }
            catch (Exception )

            {
                throw new Exception("Ошибка. Удаление отменено.");
            }
        }


        public static async Task GoalDelete(this AppDbContext context, Goal goal)
        {
            try
            {
                var accumulations = context.Accumulations.Where(c => c.GoalId == goal.Id);
                context.Accumulations.RemoveRange(accumulations);
                context.Goals.Remove(goal);
                await context.SaveChangesAsync();
            }
            catch (Exception )
            {
                throw new Exception("Ошибка. Удаление отменено.");
            }
        }
        //public static async Task UserDeleteWithCheck(this AppDbContext context, User user)
        //{
        //    try
        //    {
        //        var spendings = context.Spendings.Where(c => c.UserId == user.Id);
        //        var SpendingSubCategories = context.SpendingSubCategories.Where(c => c.UserId == user.Id);
        //        var PlanedSpendings = context.PlanedSpendings.Where(c => c.UserId == user.Id);
        //        foreach (var c in spendings)
        //        {
        //            context.Spendings.Remove(c);
        //        }
        //        foreach (var c in SpendingSubCategories)
        //        {
        //            context.SpendingSubCategories.Remove(c);
        //        }
        //        foreach (var c in PlanedSpendings)
        //        {
        //            context.PlanedSpendings.Remove(c);
        //        }
        //        context.Users.Remove(user);
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Ошибка. Удаление отменено.");
        //    }
        //}
        //public static async Task UserRoleDeleteWithCheck(this AppDbContext context, UserRole userRole)
        //{
        //    try
        //    {
        //        var users = context.Users.Where(c => c.UserRoleId == userRole.Id);

        //        foreach (var c in users)
        //        {
        //            await context.UserDeleteWithCheck(c);
        //        }

        //        context.UsersRoles.Remove(userRole);
        //        await context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Ошибка. Удаление отменено.");
        //    }
        //}
        //public static async Task DeletewithCheck<T>(this AppDbContext context, T entity) where T : class, IForDelete
        //{
        //    if (context.Set<T>().Any(x => x.Name== entity.Name&& x.Id == entity.Id))
        //        throw new Exception("Данная наименование уже существует");

        //    context.Update(entity);
        //    await context.SaveChangesAsync();
        //}
    }
}
