using System.ComponentModel.DataAnnotations;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public class MonitorConfiguration
    {
        [Required]
        public string UnknownEventsLogPath { get; set; }
            = null!;
    }
}
