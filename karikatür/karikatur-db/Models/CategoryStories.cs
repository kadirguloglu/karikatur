using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class CategoryStories
    {
        public long CategoryId { get; set; }
        public long StoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Stories Story { get; set; }
    }
}
