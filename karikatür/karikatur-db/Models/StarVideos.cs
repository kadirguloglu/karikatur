using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class StarVideos
    {
        public long StarId { get; set; }
        public long VideoId { get; set; }

        public virtual Stars Star { get; set; }
        public virtual Videos Video { get; set; }
    }
}
