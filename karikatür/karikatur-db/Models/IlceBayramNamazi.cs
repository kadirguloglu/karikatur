using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class IlceBayramNamazi
    {
        public int? IlceId { get; set; }
        public string KurbanBayramNamaziHtarihi { get; set; }
        public string KurbanBayramNamaziSaati { get; set; }
        public string KurbanBayramNamaziTarihi { get; set; }
        public string RamazanBayramNamaziHtarihi { get; set; }
        public string RamazanBayramNamaziSaati { get; set; }
        public string RamazanBayramNamaziTarihi { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Id { get; set; }
    }
}
