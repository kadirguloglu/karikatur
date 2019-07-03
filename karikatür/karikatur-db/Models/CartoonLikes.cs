using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class CartoonLikes
    {
        public Guid Id { get; set; }
        public Guid CartoonId { get; set; }
        public string UniqUserKey { get; set; }

        public virtual Cartoon Cartoon { get; set; }
    }
}
