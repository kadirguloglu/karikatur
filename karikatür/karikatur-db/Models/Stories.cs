using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Stories
    {
        public Stories()
        {
            CategoryStories = new HashSet<CategoryStories>();
        }

        public long Id { get; set; }
        public string ImageSource { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual ICollection<CategoryStories> CategoryStories { get; set; }
    }
}
