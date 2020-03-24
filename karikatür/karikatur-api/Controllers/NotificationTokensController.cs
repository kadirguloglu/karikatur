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
    public class NotificationTokensController : ControllerBase
    {
        private readonly KarikaturContext _context;

        public NotificationTokensController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: api/NotificationTokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationToken>>> GetNotificationToken()
        {
            return await _context.NotificationToken.ToListAsync();
        }

        // GET: api/NotificationTokens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationToken>> GetNotificationToken(Guid id)
        {
            var notificationToken = await _context.NotificationToken.FindAsync(id);

            if (notificationToken == null)
            {
                return NotFound();
            }

            return notificationToken;
        }

        // PUT: api/NotificationTokens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotificationToken(Guid id, NotificationToken notificationToken)
        {
            if (id != notificationToken.Id)
            {
                return BadRequest();
            }

            _context.Entry(notificationToken).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationTokenExists(id))
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

        // POST: api/NotificationTokens
        [HttpPost]
        public async Task<object> PostNotificationToken(NotificationToken notificationToken)
        {
            if (await _context.NotificationToken.AnyAsync(x => x.Device == notificationToken.Device))
            {
                return -2;
            }
            _context.NotificationToken.Add(notificationToken);
            try
            {
                var result = await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // DELETE: api/NotificationTokens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NotificationToken>> DeleteNotificationToken(Guid id)
        {
            var notificationToken = await _context.NotificationToken.FindAsync(id);
            if (notificationToken == null)
            {
                return NotFound();
            }

            _context.NotificationToken.Remove(notificationToken);
            await _context.SaveChangesAsync();

            return notificationToken;
        }

        private bool NotificationTokenExists(Guid id)
        {
            return _context.NotificationToken.Any(e => e.Id == id);
        }
    }
}
