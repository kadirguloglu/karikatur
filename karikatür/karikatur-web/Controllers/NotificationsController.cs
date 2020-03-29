using karikatur_db.ComplexType;
using karikatur_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace karikatur_web.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly KarikaturContext _context;

        public NotificationsController(KarikaturContext context)
        {
            _context = context;
        }

        // GET: Notifications
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notification.ToListAsync());
        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // GET: Notifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SendNotificationCpx notification)
        {
            notification.Id = Guid.NewGuid();
            _context.Add(notification);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                string[] tokenList = new string[] { };
                var query = _context.NotificationToken.AsQueryable();
                if (notification.IsOnlyAndroid)
                {
                    query = query.Where(x => x.Platform == "android");
                    if (notification.LastLoginWithDate != 0)
                    {
                        query = query.Where(x => x.UpdateDate.AddDays(notification.LastLoginWithDate) < DateTime.Now);
                    }
                }
                else
                {
                    if (notification.LastLoginWithDate != 0)
                    {
                        query = query.Where(x => x.UpdateDate.AddDays(notification.LastLoginWithDate) < DateTime.Now);
                    }
                }
                query.Where(x => x.Settings.ProjectKey == "KarikaturMadeni");
                tokenList = await query.Select(x => x.Token).ToArrayAsync();
                if (tokenList.Length > 0)
                {
                    RestClient client = new RestClient("https://exp.host/--/api/v2/push/send");
                    var request = new RestRequest(Method.POST);
                    client.Timeout = -1;
                    request.AddHeader("content-type", "application/json");
                    request.AddParameter("application/json", JsonConvert.SerializeObject(new
                    {
                        to = tokenList,
                        title = notification.Title,
                        body = notification.Description,
                        sound = "default"
                    }), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Notifications/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return View(notification);
        }

        // POST: Notifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description")] Notification notification)
        {
            if (id != notification.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationExists(notification.Id))
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
            return View(notification);
        }

        // GET: Notifications/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notification == null)
            {
                return NotFound();
            }

            return View(notification);
        }

        // POST: Notifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var notification = await _context.Notification.FindAsync(id);
            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationExists(Guid id)
        {
            return _context.Notification.Any(e => e.Id == id);
        }
    }
}
