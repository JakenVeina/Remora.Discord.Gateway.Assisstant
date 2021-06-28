using System;
using System.ComponentModel.DataAnnotations;

namespace Remora.Discord.Gateway.Assistant.Notifications
{
    public class NotificationsConfiguration
    {
        [Required]
        public TimeSpan Interval { get; set; }

        [Required]
        public ulong WebhookId { get; set; }

        [Required]
        public string WebhookToken { get; set; }
            = null!;
    }
}
