using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinanceAnalytic.Models.Models;
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authorization;

namespace FinanceAnalytic.Controllers.ModelControllers
{
    [Authorize]
    public class GoalsController : Controller
    {
        private readonly AppDbContext _context;

        public GoalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Goals
        public async Task<IActionResult> Index()
        {
            var goalsList = _context.Goals.Where(c=>c.User.Login==User.Identity.Name);
            return View(await goalsList.ToListAsync());
        }

        // GET: Goals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        // GET: Goals/Create
        public IActionResult Create()
        {   
            return View();
        }

        // POST: Goals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Sum")] Goal goal)
        {
            if (ModelState.IsValid)
            {
                goal.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                try
                {
                    await _context.AddWithCheckNameForUser(goal);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }            
            }
         
            return View(goal);
        }

        // GET: Goals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", goal.UserId);
            return View(goal);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Sum,UserId,IsAchived")] Goal goal)
        {
            if (id != goal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    goal.UserId = _context.Users.Where(c => c.Login == User.Identity.Name).First().Id;
                    await _context.EditwithCheckNameForUser(goal);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Login", goal.UserId);
            return View(goal);
        }

        // GET: Goals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var goal = await _context.Goals.FindAsync(id);
            await AppContextService.GoalDelete(_context,goal);
            return RedirectToAction(nameof(Index));
        }

        private bool GoalExists(int id)
        {
            return _context.Goals.Any(e => e.Id == id);
        }


        //public JsonResult ChangeIsAchived(int id, bool isAchived)
        //{
        //    try
        //    {
        //        var goal =_context.Goals.Where(c => c.Id == id).First();
        //        goal.IsAchived = isAchived;
        //        _context.Update(goal);
        //        _context.SaveChanges();
        //        return Json(true);
        //    }
        //    catch (Exception ex)
        //    {
    
        //        return Json(false);
        //    }
          
        //}
    }
}
