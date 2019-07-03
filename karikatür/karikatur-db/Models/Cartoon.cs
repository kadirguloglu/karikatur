using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Cartoon
    {
        public Cartoon()
        {
            CartoonImages = new HashSet<CartoonImages>();
            CartoonLikes = new HashSet<CartoonLikes>();
        }

        public Guid Id { get; set; }
        public Guid DrawersId { get; set; }
        public int Rank { get; set; }

        public virtual Drawer Drawers { get; set; }
        public virtual ICollection<CartoonImages> CartoonImages { get; set; }
        public virtual ICollection<CartoonLikes> CartoonLikes { get; set; }
    }
}
