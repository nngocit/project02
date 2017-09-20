using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class AppNotificationHistoryDetail
    {
        [Key]
        public long Id { get; set; }
        public string DeviceOs { get; set; }
        public string DeviceToken { get; set; }
        public string PushStatus { get; set; }
        public DateTime ProcessedDateTime { get; set; }
        public bool IsRead { get; set; }
        public virtual AppNotificationHistory AppNotificationHistory { get; set; }

    }
}
