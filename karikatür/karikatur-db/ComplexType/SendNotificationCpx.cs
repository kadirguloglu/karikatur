using karikatur_db.Models;
using System.ComponentModel;

namespace karikatur_db.ComplexType
{
    public class SendNotificationCpx : Notification
    {
        [DisplayName("Sadece android kullanıcılarına gönder")]
        public bool IsOnlyAndroid { get; set; }

        [DisplayName("Yazılan gün veya daha fazla süredir girmeyen kullanıcılara gönder. Boş geçilirse tüm kullanıcılara gönderilir.")]
        public int LastLoginWithDate { get; set; }
    }
}
