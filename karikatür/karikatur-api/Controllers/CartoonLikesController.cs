using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using karikatur_db.Models;

namespace karikatur_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartoonLikesController : ControllerBase
    {
        private readonly KarikaturContext _context;

        public CartoonLikesController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: api/CartoonLikes
        [HttpGet("{uniqUserKey}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCartoonLikes(string uniqUserKey)
        {
            return await _context.Cartoon
                .OrderByDescending(x => x.Rank)
                .Include(x => x.CartoonImages)
                .Include(x => x.Drawers)
                .Include(x => x.CartoonLikes)
                .Where(x => x.CartoonLikes.Any(y => y.UniqUserKey == uniqUserKey))
                .ToAsyncEnumerable().Select(x =>
                {
                    var likeCount = x.CartoonLikes.Count();
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
                        LikeId = likeId
                    };
                }).ToList();
        }

        // PUT: api/CartoonLikes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartoonLikes(Guid id, CartoonLikes cartoonLikes)
        {
            if (id != cartoonLikes.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartoonLikes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartoonLikesExists(id))
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

        // POST: api/CartoonLikes
        [HttpPost]
        public async Task<ActionResult<IEnumerable<object>>> PostCartoonLikes(CartoonLikes cartoonLikes)
        {
            if (CartoonLikesExists(cartoonLikes.Id))
            {
                var oldCartoonLikes = await _context.CartoonLikes.FindAsync(cartoonLikes.Id);
                if (cartoonLikes == null)
                {
                    return NotFound();
                }

                _context.CartoonLikes.Remove(oldCartoonLikes);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.CartoonLikes.Add(cartoonLikes);
                await _context.SaveChangesAsync();
            }

            return await _context.Cartoon.Where(x => x.Id == cartoonLikes.CartoonId).Include(x => x.CartoonImages).Include(x => x.Drawers).Include(x => x.CartoonLikes).ToAsyncEnumerable().Select(x =>
               {
                   var likeCount = x.CartoonLikes.Count();
                   var isLiked = x.CartoonLikes.Any(y => y.UniqUserKey == cartoonLikes.UniqUserKey);
                   var getLike = x.CartoonLikes.FirstOrDefault(y => y.UniqUserKey == cartoonLikes.UniqUserKey);
                   var likeId = new Guid();
                   if (getLike != null) likeId = getLike.Id;
                   return new
                   {
                       x.Id,
                       x.Drawers.LogoSrc,
                       x.Drawers.Name,
                       x.DrawersId,
                       CartoonImages = x.CartoonImages.Select(y => new { y.ImageSrc, y.Rank }).OrderBy(y => y.Rank),
                       LikeCount = likeCount,
                       IsLiked = isLiked,
                       LikeId = likeId
                   };
               }).ToList();
        }

        // DELETE: api/CartoonLikes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CartoonLikes>> DeleteCartoonLikes(Guid id)
        {
            var cartoonLikes = await _context.CartoonLikes.FindAsync(id);
            if (cartoonLikes == null)
            {
                return NotFound();
            }

            _context.CartoonLikes.Remove(cartoonLikes);
            await _context.SaveChangesAsync();

            return cartoonLikes;
        }

        private bool CartoonLikesExists(Guid id)
        {
            try
            {
                return _context.CartoonLikes.Any(e => e.Id == id);
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
