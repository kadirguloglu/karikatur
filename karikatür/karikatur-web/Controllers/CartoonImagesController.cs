using karikatur_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace karikatur_web.Controllers
{
    [Authorize]
    public class CartoonImagesController : Controller
    {
        private readonly KarikaturContext _context;
        private IHostingEnvironment _env;

        public CartoonImagesController(KarikaturContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: CartoonImages
        public async Task<IActionResult> Index()
        {
            var karikaturContext = _context.CartoonImages.Include(c => c.Cartoon);
            return View(await karikaturContext.ToListAsync());
        }

        // GET: CartoonImages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonImages = await _context.CartoonImages
                .Include(c => c.Cartoon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoonImages == null)
            {
                return NotFound();
            }

            return View(cartoonImages);
        }

        // GET: CartoonImages/Create
        public IActionResult Create()
        {
            ViewData["CartoonId"] = new SelectList(_context.Cartoon.OrderByDescending(x => x.Rank), "Id", "Id");
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View();
        }

        // POST: CartoonImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CartoonId,ImageSrc")] CartoonImages cartoonImages)
        {
            if (ModelState.IsValid)
            {
                cartoonImages.Id = Guid.NewGuid();
                _context.Add(cartoonImages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartoonId"] = new SelectList(_context.Cartoon.OrderByDescending(x => x.Rank), "Id", "Id", cartoonImages.CartoonId);
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View(cartoonImages);
        }

        // GET: CartoonImages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonImages = await _context.CartoonImages.FindAsync(id);
            if (cartoonImages == null)
            {
                return NotFound();
            }
            ViewData["CartoonId"] = new SelectList(_context.Cartoon.OrderByDescending(x => x.Rank), "Id", "Id", cartoonImages.CartoonId);
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View(cartoonImages);
        }

        // POST: CartoonImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,CartoonId,ImageSrc")] CartoonImages cartoonImages)
        {
            if (id != cartoonImages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartoonImages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartoonImagesExists(cartoonImages.Id))
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
            ViewData["CartoonId"] = new SelectList(_context.Cartoon.OrderByDescending(x => x.Rank), "Id", "Id", cartoonImages.CartoonId);
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View(cartoonImages);
        }

        // GET: CartoonImages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartoonImages = await _context.CartoonImages
                .Include(c => c.Cartoon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartoonImages == null)
            {
                return NotFound();
            }

            return View(cartoonImages);
        }

        // POST: CartoonImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cartoonImages = await _context.CartoonImages.FindAsync(id);
            _context.CartoonImages.Remove(cartoonImages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartoonImagesExists(Guid id)
        {
            return _context.CartoonImages.Any(e => e.Id == id);
        }
    }
}
