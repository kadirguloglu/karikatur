using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class CategoryVideos
    {
        public long CategoryId { get; set; }
        public long VideoId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Videos Video { get; set; }
    }
}
