using karikatur_db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace karikatur_web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private KarikaturContext _context;
        private IHttpContextAccessor _accessor;
        private IHostingEnvironment _env;

        //Scaffold-DbContext "Data Source=94.73.150.3;Initial Catalog=u7665680_dbF7D; User Id=u7665680_userF7D;Password=AEtx19I1" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -f
        public HomeController(KarikaturContext context, IHttpContextAccessor accessor, IHostingEnvironment env)
        {
            _context = context;
            _accessor = accessor;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UnusedPictures()
        {
            List<string> pictureUrls = await _context.CartoonImages.Select(x => x.ImageSrc).ToListAsync();
            pictureUrls.AddRange(await _context.Drawer.Select(x => x.LogoSrc).ToListAsync());
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            var unsenedPictures = new List<string>();
            for (int i = 0; i < allFiles.Count; i++)
            {
                if (!pictureUrls.Any(x => x.Contains(allFiles[i].Replace(_env.WebRootPath, ""))))
                {
                    unsenedPictures.Add(allFiles[i].Replace(_env.WebRootPath, ""));
                }
            }
            return View(unsenedPictures);
        }

        public async Task<IActionResult> BulkCartoonInsert(string message = "")
        {
            var result = await _context.Drawer.ToListAsync();
            ViewBag.message = message;
            return View(result);
        }

        [HttpPost]
        public async Task<ActionResult> BulkCartoonInsert(Guid DrawerId, List<IFormFile> CartoonImages)
        {
            try
            {
                int maxRank = await _context.Cartoon.MaxAsync(x => x.Rank);
                foreach (IFormFile file in CartoonImages)
                {
                    maxRank++;
                    var guid = Guid.NewGuid();
                    string directoryPath = $"{_env.WebRootPath}/Image/{guid}/";
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var stream = new FileStream($"{directoryPath}/{file.FileName}", FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    string imageSrc = "/Image/" + guid + "/" + file.FileName;

                    var cartoon = new Cartoon()
                    {
                        DrawersId = DrawerId,
                        Id = Guid.NewGuid(),
                        Rank = maxRank
                    };
                    await _context.Cartoon.AddAsync(cartoon);
                    await _context.SaveChangesAsync();

                    var cartoonImage = new CartoonImages()
                    {
                        Id = Guid.NewGuid(),
                        CartoonId = cartoon.Id,
                        Rank = 1,
                        ImageSrc = imageSrc
                    };
                    await _context.CartoonImages.AddAsync(cartoonImage);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("BulkCartoonInsert", new { message = ex.Message });
            }
            return RedirectToAction("BulkCartoonInsert", new { message = "İşlem başarılı" });
        }

        public async Task<IActionResult> DeleteUnusedPictures()
        {
            List<string> pictureUrls = await _context.CartoonImages.Select(x => x.ImageSrc).ToListAsync();
            pictureUrls.AddRange(await _context.Drawer.Select(x => x.LogoSrc).ToListAsync());
            var allFiles = Directory.GetFiles($"{_env.WebRootPath}/Image/", "*.*", SearchOption.AllDirectories).ToList();
            var unsenedPictures = new List<string>();
            for (int i = 0; i < allFiles.Count; i++)
            {
                if (!pictureUrls.Any(x => x.Contains(allFiles[i].Replace(_env.WebRootPath, ""))))
                {
                    string path = "";
                    string[] splitingPath = allFiles[i].Split(char.Parse(@"\"));
                    for (int x = 0; x < splitingPath.Length - 1; x++)
                    {
                        path += splitingPath[x] + "/";
                    }
                    path = path.TrimEnd('/');
                    Directory.Delete(path, true);
                }
            }
            return RedirectToAction("UnusedPictures");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ShowRequestId = true });
        }

        public IActionResult Gallery()
        {
            DirectoryInfo info = new DirectoryInfo($"{_env.WebRootPath}/Image/");
            var files = info.GetFiles("*",SearchOption.AllDirectories).OrderByDescending(p => p.CreationTime).Select(x=>x.FullName).ToList(); 
            for (int i = 0; i < files.Count; i++)
            {
                files[i] = files[i].Replace(_env.WebRootPath, "");
            } 
            return View(files);
        }

        [HttpPost]
        public async Task<JsonResult> AddFile()
        {
            try
            {
                var httpResponse = _accessor.HttpContext.Response;
                var httpRequest = _accessor.HttpContext.Request;

                var files = httpRequest.Form.Files;

                var guid = Guid.NewGuid();
                string directoryPath = $"{_env.WebRootPath}/Image/{guid}/";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var stream = new FileStream($"{directoryPath}/{files[0].FileName}", FileMode.Create))
                {
                    await files[0].CopyToAsync(stream);
                }

                return Json(new
                {
                    Status = true,
                    Link = "/Image/" + guid + "/" + files[0].FileName
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    Status = true,
                });
            }
        }

        public async Task<PartialViewResult> DemoPartialView()
        {
            return PartialView();
        }
    }
}
