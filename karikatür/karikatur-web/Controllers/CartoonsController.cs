using karikatur_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace karikatur_web.Controllers
{
    [Authorize]
    public class CartoonsController : Controller
    {
        private readonly KarikaturContext _context;

        public CartoonsController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: Cartoons
        public async Task<IActionResult> Index()
        {
            var karikaturContext = _context.Cartoon.Include(c => c.Drawers).Include(x => x.CartoonImages).OrderByDescending(x => x.Rank);
            return View(await karikaturContext.ToListAsync());
        }

        // GET: Cartoons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoon = await _context.Cartoon
                .Include(c => c.Drawers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoon == null)
            {
                return NotFound();
            }

            return View(cartoon);
        }

        // GET: Cartoons/Create
        public IActionResult Create()
        {
            ViewData["DrawersId"] = new SelectList(_context.Drawer, "Id", "Name");
            int maxRank = 0;
            if (_context.Cartoon.Any())
            {
                maxRank = _context.Cartoon.Max(x => x.Rank) + 1;
            }
            var oldCartoon = new Cartoon() { Rank = maxRank };
            return View(oldCartoon);
        }

        // POST: Cartoons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DrawersId,Rank")] Cartoon cartoon)
        {
            if (ModelState.IsValid)
            {
                cartoon.Id = Guid.NewGuid();
                _context.Add(cartoon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DrawersId"] = new SelectList(_context.Drawer, "Id", "Name", cartoon.DrawersId);
            return View(cartoon);
        }

        // GET: Cartoons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoon = await _context.Cartoon.FindAsync(id);
            if (cartoon == null)
            {
                return NotFound();
            }
            ViewData["DrawersId"] = new SelectList(_context.Drawer, "Id", "Name", cartoon.DrawersId);
            return View(cartoon);
        }

        // POST: Cartoons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DrawersId,Rank")] Cartoon cartoon)
        {
            if (id != cartoon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartoon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartoonExists(cartoon.Id))
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
            ViewData["DrawersId"] = new SelectList(_context.Drawer, "Id", "Name", cartoon.DrawersId);
            return View(cartoon);
        }

        // GET: Cartoons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoon = await _context.Cartoon
                .Include(c => c.Drawers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoon == null)
            {
                return NotFound();
            }

            return View(cartoon);
        }

        // POST: Cartoons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cartoon = await _context.Cartoon.FindAsync(id);
            _context.Cartoon.Remove(cartoon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartoonExists(Guid id)
        {
            return _context.Cartoon.Any(e => e.Id == id);
        }
    }
}
