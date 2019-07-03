using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using karikatur_db.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace karikatur_web.Controllers
{
    [Authorize]
    public class DrawersController : Controller
    {
        private readonly KarikaturContext _context;
        private IHostingEnvironment _env;

        public DrawersController(KarikaturContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Drawers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Drawer.ToListAsync());
        }

        // GET: Drawers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drawer = await _context.Drawer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drawer == null)
            {
                return NotFound();
            }

            return View(drawer);
        }

        // GET: Drawers/Create
        public IActionResult Create()
        {
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View();
        }

        // POST: Drawers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LogoSrc,Name")] Drawer drawer)
        {
            if (ModelState.IsValid)
            {
                drawer.Id = Guid.NewGuid();
                _context.Add(drawer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View(drawer);
        }

        // GET: Drawers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drawer = await _context.Drawer.FindAsync(id);
            if (drawer == null)
            {
                return NotFound();
            }
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View(drawer);
        }

        // POST: Drawers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,LogoSrc,Name")] Drawer drawer)
        {
            if (id != drawer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drawer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrawerExists(drawer.Id))
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
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < allFiles.Count; i++)
            {
                allFiles[i] = allFiles[i].Replace(_env.WebRootPath, "");
            }
            ViewData["allFiles"] = allFiles;
            return View(drawer);
        }

        // GET: Drawers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drawer = await _context.Drawer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drawer == null)
            {
                return NotFound();
            }

            return View(drawer);
        }

        // POST: Drawers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var drawer = await _context.Drawer.FindAsync(id);
            _context.Drawer.Remove(drawer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrawerExists(Guid id)
        {
            return _context.Drawer.Any(e => e.Id == id);
        }
    }
}
