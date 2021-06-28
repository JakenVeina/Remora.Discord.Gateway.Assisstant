using System.ComponentModel.DataAnnotations;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public class MonitorConfiguration
    {
        [Required]
        public string EventsLogPath { get; set; }
            = null!;
    }
}
