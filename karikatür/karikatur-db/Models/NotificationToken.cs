using System;
using System.Collections.Generic;

namespace karikatur_db.Models
{
    public partial class NotificationToken
    {
        public NotificationToken()
        {
            EzanVakitBildirim = new HashSet<EzanVakitBildirim>();
        }

        public Guid Id { get; set; }
        public string Token { get; set; }
        public string Device { get; set; }
        public DateTime CreateDate { get; set; }
        public string Platform { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid? SettingsId { get; set; }

        public virtual Settings Settings { get; set; }
        public virtual ICollection<EzanVakitBildirim> EzanVakitBildirim { get; set; }
    }
}
