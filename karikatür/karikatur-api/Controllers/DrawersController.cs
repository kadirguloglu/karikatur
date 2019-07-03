using karikatur_db.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace karikatur_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawersController : ControllerBase
    {
        private readonly KarikaturContext _context;

        public DrawersController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: api/Drawers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drawer>>> GetDrawer()
        {
            return await _context.Drawer.ToListAsync();
        }

        // GET: api/Drawers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drawer>> GetDrawer(Guid id)
        {
            var drawer = await _context.Drawer.FindAsync(id);

            if (drawer == null)
            {
                return NotFound();
            }

            return drawer;
        }

        // PUT: api/Drawers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrawer(Guid id, Drawer drawer)
        {
            if (id != drawer.Id)
            {
                return BadRequest();
            }

            _context.Entry(drawer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrawerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Drawers
        [HttpPost]
        public async Task<ActionResult<Drawer>> PostDrawer(Drawer drawer)
        {
            _context.Drawer.Add(drawer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDrawer", new { id = drawer.Id }, drawer);
        }

        // DELETE: api/Drawers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drawer>> DeleteDrawer(Guid id)
        {
            var drawer = await _context.Drawer.FindAsync(id);
            if (drawer == null)
            {
                return NotFound();
            }

            _context.Drawer.Remove(drawer);
            await _context.SaveChangesAsync();

            return drawer;
        }

        private bool DrawerExists(Guid id)
        {
            return _context.Drawer.Any(e => e.Id == id);
        }
    }
}
