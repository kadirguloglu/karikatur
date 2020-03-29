using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Ulkeler
    {
        public int? UlkeId { get; set; }
        public string UlkeAdiEn { get; set; }
        public string UlkeAdi { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Id { get; set; }
    }
}
