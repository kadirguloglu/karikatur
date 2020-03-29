using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using karikatur_db.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace karikatur_api.Controllers
{
    [Route("api/EzanVakit/[controller]")]
    [ApiController]
    public class VakitlerController : ControllerBase
    {
        private readonly KarikaturContext _context;
        public VakitlerController(KarikaturContext context)
        {
            _context = context;
        }

        [HttpGet("{ilceId}")]
        public async Task<Vakitler> GetVakitler(int ilceId)
        {
            if (await _context.Vakitler.AnyAsync(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.IlceId == ilceId && x.MiladiTarihUzunIso8601.Value.ToString("DD.MM.YYYY") == DateTime.Now.ToString("DD.MM.YYYY")))
            {
                return await _context.Vakitler.Where(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.IlceId == ilceId && x.MiladiTarihUzunIso8601.Value.ToString("DD.MM.YYYY") == DateTime.Now.ToString("DD.MM.YYYY")).FirstOrDefaultAsync();
            }
            var client = new RestClient("http://ezanvakti.herokuapp.com/vakitler?ilce=" + ilceId);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var apiResult = JsonConvert.DeserializeObject<List<Vakitler>>(response.Content);
            apiResult.ForEach(x => { x.LastUpdateDate = DateTime.Now; x.IlceId = ilceId; });
            _context.Vakitler.AddRange(apiResult);
            _context.SaveChanges();
            return apiResult.FirstOrDefault(x => x.IlceId == ilceId && x.MiladiTarihUzunIso8601.Value.ToString("DD.MM.YYYY") == DateTime.Now.ToString("DD.MM.YYYY"));
        }
    } 
}
