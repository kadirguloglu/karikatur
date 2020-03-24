using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Videos
    {
        public Videos()
        {
            CategoryVideos = new HashSet<CategoryVideos>();
            StarVideos = new HashSet<StarVideos>();
            VideoTagVideos = new HashSet<VideoTagVideos>();
            Watchings = new HashSet<Watchings>();
        }

        public long Id { get; set; }
        public string UniqKey { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDesktop { get; set; }
        public int Second { get; set; }
        public string Quality { get; set; }
        public string VideoSource { get; set; }
        public string PreviewImageSource { get; set; }
        public string PreviewVideoSource { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? IsPublish { get; set; }

        public virtual ICollection<CategoryVideos> CategoryVideos { get; set; }
        public virtual ICollection<StarVideos> StarVideos { get; set; }
        public virtual ICollection<VideoTagVideos> VideoTagVideos { get; set; }
        public virtual ICollection<Watchings> Watchings { get; set; }
    }
}
