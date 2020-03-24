using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Stars
    {
        public Stars()
        {
            StarVideos = new HashSet<StarVideos>();
        }

        public long Id { get; set; }
        public string ImageSource { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<StarVideos> StarVideos { get; set; }
    }
}
