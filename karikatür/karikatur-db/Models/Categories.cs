using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Categories
    {
        public Categories()
        {
            CategoryStories = new HashSet<CategoryStories>();
            CategoryVideos = new HashSet<CategoryVideos>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<CategoryStories> CategoryStories { get; set; }
        public virtual ICollection<CategoryVideos> CategoryVideos { get; set; }
    }
}
