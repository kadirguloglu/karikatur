using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class VideoTags
    {
        public VideoTags()
        {
            VideoTagVideos = new HashSet<VideoTagVideos>();
        }

        public long Id { get; set; }
        public string TagText { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<VideoTagVideos> VideoTagVideos { get; set; }
    }
}
