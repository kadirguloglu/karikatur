﻿using System;
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
    public class SettingsController : ControllerBase
    {
        private readonly KarikaturContext _context;

        public SettingsController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: api/Settings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Settings>>> GetSettings()
        {
            return await _context.Settings.ToListAsync();
        }

        // GET: api/Settings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Settings>> GetSettings(Guid id)
        {
            var settings = await _context.Settings.FindAsync(id);

            if (settings == null)
            {
                return NotFound();
            }

            return settings;
        }

        [HttpGet("GetProjectDetail/{projectKey}")]
        public async Task<object> GetProjectDetail(string projectKey)
        {
            var settings = await _context.Settings.FirstOrDefaultAsync(x => x.ProjectKey == projectKey);

            if (settings == null)
            {
                return "error";
            }

            return settings;
        }

        // PUT: api/Settings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSettings(Guid id, Settings settings)
        {
            if (id != settings.Id)
            {
                return BadRequest();
            }

            _context.Entry(settings).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingsExists(id))
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

        // POST: api/Settings
        [HttpPost]
        public async Task<ActionResult<Settings>> PostSettings(Settings settings)
        {
            _context.Settings.Add(settings);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SettingsExists(settings.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSettings", new { id = settings.Id }, settings);
        }

        // DELETE: api/Settings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Settings>> DeleteSettings(Guid id)
        {
            var settings = await _context.Settings.FindAsync(id);
            if (settings == null)
            {
                return NotFound();
            }

            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();

            return settings;
        }

        private bool SettingsExists(Guid id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}
