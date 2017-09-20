using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebComment.Data
{
    public class AppTokenFailed
    {
        [Key]
        public long Id { get; set; }

        public string DeviceOs { get; set; }
        public string DeviceToken { get; set; }
        public string ExMessage { get; set; }
        public DateTime? DateProcessed { get; set; }
        public long AppNotificationHistoryId { get; set; }
    }
}
