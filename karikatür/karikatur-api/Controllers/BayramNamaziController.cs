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
    public class BayramNamaziController : ControllerBase
    {
        private readonly KarikaturContext _context;
        public BayramNamaziController(KarikaturContext context)
        {
            _context = context;
        }

        [HttpGet("{ilceId}")]
        public async Task<IlceBayramNamazi> IlceBayramNamaziGetir(int ilceId)
        {
            if (await _context.IlceBayramNamazi.AnyAsync(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.IlceId == ilceId))
            {
                return await _context.IlceBayramNamazi.Where(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.IlceId == ilceId).FirstOrDefaultAsync();
            }
            var client = new RestClient("http://ezanvakti.herokuapp.com/bayram?ilce=" + ilceId);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var apiResult = JsonConvert.DeserializeObject<IlceBayramNamazi>(response.Content);
            apiResult.LastUpdateDate = DateTime.Now;
            apiResult.IlceId = ilceId;
            _context.IlceBayramNamazi.AddRange(apiResult);
            _context.SaveChanges();
            return apiResult;
        }
    }
}
