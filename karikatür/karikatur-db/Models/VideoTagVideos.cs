using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class VideoTagVideos
    {
        public long VideoTagId { get; set; }
        public long VideoId { get; set; }

        public virtual Videos Video { get; set; }
        public virtual VideoTags VideoTag { get; set; }
    }
}
