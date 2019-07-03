using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class CartoonImages
    {
        public Guid Id { get; set; }
        public Guid CartoonId { get; set; }
        public string ImageSrc { get; set; }
        public int Rank { get; set; }

        public virtual Cartoon Cartoon { get; set; }
    }
}
