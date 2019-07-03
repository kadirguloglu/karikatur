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
    public class CartoonImagesController : ControllerBase
    {
        private readonly KarikaturContext _context;

        public CartoonImagesController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: api/CartoonImages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartoonImages>>> GetCartoonImages()
        {
            return await _context.CartoonImages.ToListAsync();
        }

        // GET: api/CartoonImages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartoonImages>> GetCartoonImages(Guid id)
        {
            var cartoonImages = await _context.CartoonImages.FindAsync(id);

            if (cartoonImages == null)
            {
                return NotFound();
            }

            return cartoonImages;
        }

        // PUT: api/CartoonImages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartoonImages(Guid id, CartoonImages cartoonImages)
        {
            if (id != cartoonImages.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartoonImages).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartoonImagesExists(id))
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

        // POST: api/CartoonImages
        [HttpPost]
        public async Task<ActionResult<CartoonImages>> PostCartoonImages(CartoonImages cartoonImages)
        {
            _context.CartoonImages.Add(cartoonImages);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartoonImages", new { id = cartoonImages.Id }, cartoonImages);
        }

        // DELETE: api/CartoonImages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CartoonImages>> DeleteCartoonImages(Guid id)
        {
            var cartoonImages = await _context.CartoonImages.FindAsync(id);
            if (cartoonImages == null)
            {
                return NotFound();
            }

            _context.CartoonImages.Remove(cartoonImages);
            await _context.SaveChangesAsync();

            return cartoonImages;
        }

        private bool CartoonImagesExists(Guid id)
        {
            return _context.CartoonImages.Any(e => e.Id == id);
        }
    }
}
