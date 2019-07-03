using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Drawer
    {
        public Drawer()
        {
            Cartoon = new HashSet<Cartoon>();
        }

        public Guid Id { get; set; }
        public string LogoSrc { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Cartoon> Cartoon { get; set; }
    }
}
