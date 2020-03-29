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
    public class UlkelerController : ControllerBase
    {
        private readonly KarikaturContext _context;
        public UlkelerController(KarikaturContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Ulkeler>> GetUlkeler()
        {
            if (await _context.Ulkeler.AnyAsync(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month))
            {
                return await _context.Ulkeler.Where(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month).ToListAsync();
            }
            var client = new RestClient("http://ezanvakti.herokuapp.com/ulkeler");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var apiResult = JsonConvert.DeserializeObject<List<Ulkeler>>(response.Content);
            apiResult.ForEach(x => x.LastUpdateDate = DateTime.Now);
            _context.Ulkeler.AddRange(apiResult);
            _context.SaveChanges();
            return apiResult;
        }
    }
}
