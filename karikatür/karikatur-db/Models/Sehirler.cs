using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Sehirler
    {
        public int? SehirId { get; set; }
        public string SehirAdiEn { get; set; }
        public string SehirAdi { get; set; }
        public int? UlkeId { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Id { get; set; }
    }
}
