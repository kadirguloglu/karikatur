using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Vakitler
    {
        public int? IlceId { get; set; }
        public string Aksam { get; set; }
        public string AyinSekliUrl { get; set; }
        public string Gunes { get; set; }
        public string GunesBatis { get; set; }
        public string GunesDogus { get; set; }
        public string HicriTarihKisa { get; set; }
        public string HicriTarihUzun { get; set; }
        public string Ikindi { get; set; }
        public string Imsak { get; set; }
        public string KibleSaati { get; set; }
        public string MiladiTarihKisa { get; set; }
        public string MiladiTarihKisaIso8601 { get; set; }
        public string MiladiTarihUzun { get; set; }
        public DateTime? MiladiTarihUzunIso8601 { get; set; }
        public string Ogle { get; set; }
        public string Yatsi { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Id { get; set; }
    }
}
