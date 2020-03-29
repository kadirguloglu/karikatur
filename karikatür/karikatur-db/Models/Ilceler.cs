using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Ilceler
    {
        public int? IlceId { get; set; }
        public string IlceAdiEn { get; set; }
        public string IlceAdi { get; set; }
        public int? SehirId { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Id { get; set; }
    }
}
