using karikatur_db.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace karikatur_web.Controllers
{
    public class LoginController : Controller
    {
        private readonly KarikaturContext _context;

        public LoginController(KarikaturContext context)
        {
            _context = context;
        }

        public IActionResult Index(int pageType = -1, string Email = "", bool GetPassword = false, string ErrorText = "")
        {
            ViewBag.GetPassword = GetPassword;
            ViewBag.Email = Email;
            ViewBag.pageType = pageType;
            ViewBag.ErrorText = ErrorText;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendPassword(Users users)
        {
            var password = Guid.NewGuid();
            var user = _context.Users.FirstOrDefault(x => x.Email == users.Email);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                user.Password = password;
                user.PasswordExpirationDate = DateTime.Now.AddMinutes(10);
                _context.Update(user);
                if (await _context.SaveChangesAsync() > 0)
                {

                    var fromAddress = new MailAddress("exceptionlist1@gmail.com", "Karikatür - Admin Parola" + DateTime.Now.ToString());
                    var toAddress = new MailAddress(users.Email, "Parola");

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, "xz06pl54Cer32.")
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = "Parolanız - " + DateTime.Now.ToString(),
                        Body = "<p style='font-size:25px'>" + password + "</p>",
                        IsBodyHtml = true
                    })
                        try
                        {
                            smtp.Send(message);
                        }
                        catch (Exception err)
                        {
                            return RedirectToAction("Index", new { pageType = 1, Email = users.Email, ErrorText = err.Message });
                        }
                    return RedirectToAction("Index", new { pageType = 0, Email = users.Email, GetPassword = true });
                }
            }
            return RedirectToAction("Index", new { pageType = 2, Email = users.Email });
        }

        [HttpPost]
        public async Task<ActionResult> LoginUser(Users users)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == users.Email);
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                if (DateTime.Now > user.PasswordExpirationDate)
                {
                    return RedirectToAction("Index", new { pageType = 4, Email = users.Email, GetPassword = false });
                }
                if (users.Password == user.Password)
                {
                    var claims = new List<Claim>();

                    try
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Email));
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

                        var principal = new ClaimsPrincipal(identity);

                        var authProperties = new AuthenticationProperties
                        {
                            AllowRefresh = true,
                            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                            IsPersistent = true,
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);
                    }
                    catch (Exception err)
                    {
                        return RedirectToAction("Index", new { pageType = 99, Email = users.Email, ErrorText = err.Message });
                    }

                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", new { pageType = 3, Email = users.Email, GetPassword = true });
            }
            return RedirectToAction("Index", new { pageType = 2, Email = users.Email });
        }
    }
}