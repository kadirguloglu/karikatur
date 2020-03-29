using karikatur_db.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TM = System.Timers;

namespace karikatur_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private static TM.Timer aTimer;
        private static KarikaturContext _context { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<KarikaturContext>(opt =>
            {

            });
            services
                 .AddMvc()
                 .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                     options.SerializerSettings.ContractResolver = null;
                 })
                 .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            _context = new KarikaturContext();
            SetTimer();
            SendNotificationWithLast6Day();
        }
        private static void SendNotificationWithLast6Day()
        {
            aTimer = new TM.Timer(new TimeSpan(70, 0, 0).TotalMilliseconds);
            aTimer.Elapsed += ATimer_Elapsed;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void ATimer_Elapsed(object sender, TM.ElapsedEventArgs e)
        {
            var tokens = _context.NotificationToken.Where(x => x.UpdateDate.AddDays(6) < DateTime.Now).Select(x => x.Token).ToArray();
            RestClient client = new RestClient("https://exp.host/--/api/v2/push/send");
            var request = new RestRequest(Method.POST);
            client.Timeout = -1;
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(new
            {
                to = tokens,
                title = "Seni Özledik.",
                body = "Uygulamamızdaki en yeni özellikleri görmek için hemen tıkla.",
                sound = "default"
            }), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        private static void SetTimer()
        {
            aTimer = new TM.Timer(new TimeSpan(0, 0, 25).TotalMilliseconds);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, TM.ElapsedEventArgs e)
        {
            var tokens = _context.EzanVakitBildirim
                .Include(x => x.NotificationToken).ToList();

            foreach (var item in tokens.Select(x => x.IlceId).GroupBy(x => x))
            {
                var günSaatleri = new Vakitler();
                if (_context.Vakitler.Any(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.IlceId == item.Key && x.MiladiTarihUzunIso8601.Value.ToString("DD.MM.YYYY") == DateTime.Now.ToString("DD.MM.YYYY")))
                {
                    günSaatleri = _context.Vakitler.Where(x => x.LastUpdateDate.Year == DateTime.Now.Year && x.LastUpdateDate.Month == DateTime.Now.Month && x.IlceId == item.Key && x.MiladiTarihUzunIso8601.Value.ToString("DD.MM.YYYY") == DateTime.Now.ToString("DD.MM.YYYY")).FirstOrDefault();
                }
                else
                {
                    var client = new RestClient("http://ezanvakti.herokuapp.com/vakitler?ilce=" + item.Key)
                    {
                        Timeout = -1
                    };
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    var apiResult = JsonConvert.DeserializeObject<List<Vakitler>>(response.Content);
                    apiResult.ForEach(x => { x.LastUpdateDate = DateTime.Now; x.IlceId = item.Key; });
                    _context.Vakitler.AddRange(apiResult);
                    _context.SaveChanges();
                    günSaatleri = apiResult.FirstOrDefault(x => x.IlceId == item.Key && x.MiladiTarihUzunIso8601.Value.ToString("DD.MM.YYYY") == DateTime.Now.ToString("DD.MM.YYYY"));
                }

                var splitImsak = günSaatleri.Imsak.Split(':');
                TimeSpan imsakNamazi = new TimeSpan(Convert.ToInt32(splitImsak[0]), Convert.ToInt32(splitImsak[1]), 0);

                var splitOgle = günSaatleri.Ogle.Split(':');
                TimeSpan ogleNamazi = new TimeSpan(Convert.ToInt32(splitOgle[0]), Convert.ToInt32(splitOgle[1]), 0);

                var splitAksam = günSaatleri.Aksam.Split(':');
                TimeSpan aksamNamazi = new TimeSpan(Convert.ToInt32(splitAksam[0]), Convert.ToInt32(splitAksam[1]), 0);

                var splitIkindi = günSaatleri.Ikindi.Split(':');
                TimeSpan ikindiNamazi = new TimeSpan(Convert.ToInt32(splitIkindi[0]), Convert.ToInt32(splitIkindi[1]), 0);

                var splitYatsi = günSaatleri.Yatsi.Split(':');
                TimeSpan yatsiNamazi = new TimeSpan(Convert.ToInt32(splitYatsi[0]), Convert.ToInt32(splitYatsi[1]), 0);

                foreach (var kullanici in tokens.Where(x => x.IlceId == item.Key))
                {
                    string message = "";
                    if (DateTime.Now.Hour < imsakNamazi.Hours && DateTime.Now.Minute < imsakNamazi.Minutes)
                    {
                        var betweenMinute = ogleNamazi - new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if (kullanici.Timer == betweenMinute.TotalMinutes)
                        {
                            message = "Sabah namazına " + kullanici.Timer + " dakika kaldı.";
                        }
                    }
                    if (DateTime.Now.Hour > imsakNamazi.Hours && DateTime.Now.Minute > imsakNamazi.Minutes && DateTime.Now.Hour < ogleNamazi.Hours && DateTime.Now.Minute < ogleNamazi.Minutes)
                    {
                        var betweenMinute = ogleNamazi - new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if (kullanici.Timer == betweenMinute.TotalMinutes)
                        {
                            message = "Öğlen namazına " + kullanici.Timer + " dakika kaldı.";
                        }
                    }

                    if (DateTime.Now.Hour > ogleNamazi.Hours && DateTime.Now.Minute > ogleNamazi.Minutes && DateTime.Now.Hour < aksamNamazi.Hours && DateTime.Now.Minute < aksamNamazi.Minutes)
                    {
                        var betweenMinute = aksamNamazi - new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if (kullanici.Timer == betweenMinute.TotalMinutes)
                        {
                            message = "Akşam namazına " + kullanici.Timer + " dakika kaldı.";
                        }
                    }

                    if (DateTime.Now.Hour > aksamNamazi.Hours && DateTime.Now.Minute > aksamNamazi.Minutes && DateTime.Now.Hour < ikindiNamazi.Hours && DateTime.Now.Minute < ikindiNamazi.Minutes)
                    {
                        var betweenMinute = ikindiNamazi - new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if (kullanici.Timer == betweenMinute.TotalMinutes)
                        {
                            message = "İkindi namazına " + kullanici.Timer + " dakika kaldı.";
                        }
                    }

                    if (DateTime.Now.Hour > ikindiNamazi.Hours && DateTime.Now.Minute > ikindiNamazi.Minutes && DateTime.Now.Hour < yatsiNamazi.Hours && DateTime.Now.Minute < yatsiNamazi.Minutes)
                    {
                        var betweenMinute = yatsiNamazi - new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if (kullanici.Timer == betweenMinute.TotalMinutes)
                        {
                            message = "Yatsı namazına " + kullanici.Timer + " dakika kaldı.";
                        }
                    }

                    new Thread(() =>
                    {
                        RestClient client = new RestClient("https://exp.host/--/api/v2/push/send");
                        var request = new RestRequest(Method.POST);
                        client.Timeout = -1;
                        request.AddHeader("content-type", "application/json");
                        request.AddParameter("application/json", JsonConvert.SerializeObject(new
                        {
                            to = kullanici.NotificationToken.Token,
                            title = "Ezan vakti hatırlatması",
                            body = message,
                            sound = "default"
                        }), ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                    });
                }
            }
        }
    }
}
