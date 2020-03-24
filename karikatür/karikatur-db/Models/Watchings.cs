using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Watchings
    {
        public long Id { get; set; }
        public string UniqKey { get; set; }
        public DateTime CreateDate { get; set; }
        public long VideoRefId { get; set; }

        public virtual Videos VideoRef { get; set; }
    }
}
