using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using karikatur_db.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

namespace karikatur_api.Controllers
{
    [Route("api/EzanVakit/[controller]")]
    [ApiController]
    public class IlcelerController : ControllerBase
    {
        private readonly KarikaturContext _context;
        public IlcelerController(KarikaturContext context)
        {
            _context = context;
        }

        [HttpGet("{sehirId}")]
        public async Task<List<Ilceler>> GetIlcelers(int sehirId)
        {
            if (await _context.Ilceler.AnyAsync(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.SehirId == sehirId))
            {
                return await _context.Ilceler.Where(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.SehirId == sehirId).ToListAsync();
            }
            var client = new RestClient("http://ezanvakti.herokuapp.com/ilceler?sehir=" + sehirId);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var apiResult = JsonConvert.DeserializeObject<List<Ilceler>>(response.Content);
            apiResult.ForEach(x => { x.LastUpdateDate = DateTime.Now; x.SehirId = sehirId; });
            _context.Ilceler.AddRange(apiResult);
            _context.SaveChanges();
            return apiResult;
        }
    }
}
