using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class AppNotificationHistory
    {
        public AppNotificationHistory()
        {
            this.ListAppNotificationHistoryDetail = new HashSet<AppNotificationHistoryDetail>();
        }
        [Key]
        public long Id { get; set; }

        public string PushType { get; set; }
        public string MessageType { get; set; }
        public string MessageValue { get; set; }
        public string Alert { get; set; }
        public string UserPushed { get; set; }
        public DateTime? StartProcessDateTime { get; set; }
        public DateTime? EndProcessAtDateTime { get; set; }
        public int? TotalDevice { get; set; }

        public int? TotalAndroidToPush { get; set; }
        public int? TotalAndroidFail { get; set; }

        public int? TotalIOsToPush { get; set; }
        public int? TotalIOsFail { get; set; }

        public int? NumFailed { get; set; }
        public string CurrentStatus { get; set; }

        public virtual ICollection<AppNotificationHistoryDetail> ListAppNotificationHistoryDetail { get; set; }

    }
}
