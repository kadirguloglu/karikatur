using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class Notification
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
