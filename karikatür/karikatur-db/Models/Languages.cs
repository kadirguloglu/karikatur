using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Languages
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string FlagSource { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
