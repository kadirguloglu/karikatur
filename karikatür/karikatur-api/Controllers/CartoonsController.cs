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
    public class CartoonsController : ControllerBase
    {
        private readonly KarikaturContext _context;

        public CartoonsController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: api/Cartoons
        [HttpGet("{page}/{count}/{uniqUserKey}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCartoon(int page, int count, string uniqUserKey)
        {
            try
            {
                if (page < 1)
                {
                    page = await _context.Cartoon.CountAsync() + page;
                }
            }
            catch (Exception)
            {

            }
            return await _context.Cartoon.OrderByDescending(x => x.Rank).Include(x => x.CartoonImages).Include(x => x.Drawers).Include(x => x.CartoonLikes).Skip((page - 1) * count).Take(count).ToAsyncEnumerable().Select(x =>
              {
                  var likeCount = x.CartoonLikes.Count();
                  var isLiked = x.CartoonLikes.Any(y => y.UniqUserKey == uniqUserKey);
                  var getLike = x.CartoonLikes.FirstOrDefault(y => y.UniqUserKey == uniqUserKey);
                  var likeId = new Guid();
                  if (getLike != null) likeId = getLike.Id;
                  return new
                  {
                      x.Id,
                      x.Drawers.LogoSrc,
                      x.Drawers.Name,
                      x.DrawersId,
                      CartoonImages = x.CartoonImages.Select(y => new { y.ImageSrc, y.Rank }).OrderByDescending(y => y.Rank),
                      LikeCount = likeCount,
                      IsLiked = isLiked,
                      LikeId = likeId
                  };
              }).ToList();
        }

        // GET: api/Cartoons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cartoon>> GetCartoon(Guid id)
        {
            var cartoon = await _context.Cartoon.FindAsync(id);

            if (cartoon == null)
            {
                return NotFound();
            }

            return cartoon;
        }

        // PUT: api/Cartoons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartoon(Guid id, Cartoon cartoon)
        {
            if (id != cartoon.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartoon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartoonExists(id))
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

        // POST: api/Cartoons
        [HttpPost]
        public async Task<ActionResult<Cartoon>> PostCartoon(Cartoon cartoon)
        {
            _context.Cartoon.Add(cartoon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartoon", new { id = cartoon.Id }, cartoon);
        }

        // DELETE: api/Cartoons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cartoon>> DeleteCartoon(Guid id)
        {
            var cartoon = await _context.Cartoon.FindAsync(id);
            if (cartoon == null)
            {
                return NotFound();
            }

            _context.Cartoon.Remove(cartoon);
            await _context.SaveChangesAsync();

            return cartoon;
        }

        private bool CartoonExists(Guid id)
        {
            return _context.Cartoon.Any(e => e.Id == id);
        }
    }
}
