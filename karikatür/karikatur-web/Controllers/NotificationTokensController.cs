using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using karikatur_db.Models;

namespace karikatur_web.Controllers
{
    public class NotificationTokensController : Controller
    {
        private readonly KarikaturContext _context;

        public NotificationTokensController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: NotificationTokens
        public async Task<IActionResult> Index()
        {
            return View(await _context.NotificationToken.ToListAsync());
        }

        // GET: NotificationTokens/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationToken = await _context.NotificationToken
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificationToken == null)
            {
                return NotFound();
            }

            return View(notificationToken);
        }

        // GET: NotificationTokens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationTokens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Token,Device,CreateDate")] NotificationToken notificationToken)
        {
            if (ModelState.IsValid)
            {
                notificationToken.Id = Guid.NewGuid();
                _context.Add(notificationToken);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationToken);
        }

        // GET: NotificationTokens/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationToken = await _context.NotificationToken.FindAsync(id);
            if (notificationToken == null)
            {
                return NotFound();
            }
            return View(notificationToken);
        }

        // POST: NotificationTokens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Token,Device,CreateDate")] NotificationToken notificationToken)
        {
            if (id != notificationToken.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationToken);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationTokenExists(notificationToken.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notificationToken);
        }

        // GET: NotificationTokens/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notificationToken = await _context.NotificationToken
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notificationToken == null)
            {
                return NotFound();
            }

            return View(notificationToken);
        }

        // POST: NotificationTokens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var notificationToken = await _context.NotificationToken.FindAsync(id);
            _context.NotificationToken.Remove(notificationToken);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationTokenExists(Guid id)
        {
            return _context.NotificationToken.Any(e => e.Id == id);
        }
    }
}
