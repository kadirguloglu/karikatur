using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class EzanVakitBildirim
    {
        public Guid Id { get; set; }
        public Guid NotificationTokenId { get; set; }
        public int IlceId { get; set; }
        public int Timer { get; set; }

        public virtual NotificationToken NotificationToken { get; set; }
    }
}
