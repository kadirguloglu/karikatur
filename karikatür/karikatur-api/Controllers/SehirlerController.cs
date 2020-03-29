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
    public class SehirlerController : ControllerBase
    {
        private readonly KarikaturContext _context;
        public SehirlerController(KarikaturContext context)
        {
            _context = context;
        }

        [HttpGet("{ulkeId}")]
        public async Task<List<Sehirler>> GetSehirlers(int ulkeId)
        {
            if (await _context.Sehirler.AnyAsync(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.UlkeId == ulkeId))
            {
                return await _context.Sehirler.Where(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.UlkeId == ulkeId).ToListAsync();
            }
            var client = new RestClient("http://ezanvakti.herokuapp.com/sehirler?ulke=" + ulkeId);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var apiResult = JsonConvert.DeserializeObject<List<Sehirler>>(response.Content);
            apiResult.ForEach(x => { x.LastUpdateDate = DateTime.Now; x.UlkeId = ulkeId; });
            _context.Sehirler.AddRange(apiResult);
            _context.SaveChanges();
            return apiResult;
        }
    }
}
