using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using karikatur_db.Models;
using Microsoft.AspNetCore.Authorization;

namespace karikatur_web.Controllers
{
    [Authorize]
    public class CartoonLikesController : Controller
    {
        private readonly KarikaturContext _context;

        public CartoonLikesController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: CartoonLikes
        public async Task<IActionResult> Index()
        {
            var karikaturContext = _context.CartoonLikes.Include(c => c.Cartoon);
            return View(await karikaturContext.ToListAsync());
        }

        // GET: CartoonLikes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonLikes = await _context.CartoonLikes
                .Include(c => c.Cartoon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoonLikes == null)
            {
                return NotFound();
            }

            return View(cartoonLikes);
        }

        // GET: CartoonLikes/Create
        public IActionResult Create()
        {
            ViewData["CartoonId"] = new SelectList(_context.Cartoon, "Id", "Id");
            return View();
        }

        // POST: CartoonLikes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CartoonId,UniqUserKey")] CartoonLikes cartoonLikes)
        {
            if (ModelState.IsValid)
            {
                cartoonLikes.Id = Guid.NewGuid();
                _context.Add(cartoonLikes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartoonId"] = new SelectList(_context.Cartoon, "Id", "Id", cartoonLikes.CartoonId);
            return View(cartoonLikes);
        }

        // GET: CartoonLikes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonLikes = await _context.CartoonLikes.FindAsync(id);
            if (cartoonLikes == null)
            {
                return NotFound();
            }
            ViewData["CartoonId"] = new SelectList(_context.Cartoon, "Id", "Id", cartoonLikes.CartoonId);
            return View(cartoonLikes);
        }

        // POST: CartoonLikes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CartoonId,UniqUserKey")] CartoonLikes cartoonLikes)
        {
            if (id != cartoonLikes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartoonLikes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartoonLikesExists(cartoonLikes.Id))
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
            ViewData["CartoonId"] = new SelectList(_context.Cartoon, "Id", "Id", cartoonLikes.CartoonId);
            return View(cartoonLikes);
        }

        // GET: CartoonLikes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonLikes = await _context.CartoonLikes
                .Include(c => c.Cartoon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoonLikes == null)
            {
                return NotFound();
            }

            return View(cartoonLikes);
        }

        // POST: CartoonLikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cartoonLikes = await _context.CartoonLikes.FindAsync(id);
            _context.CartoonLikes.Remove(cartoonLikes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartoonLikesExists(Guid id)
        {
            return _context.CartoonLikes.Any(e => e.Id == id);
        }
    }
}
