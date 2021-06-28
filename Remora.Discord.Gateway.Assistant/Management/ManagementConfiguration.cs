using System.ComponentModel.DataAnnotations;

namespace Remora.Discord.Gateway.Assistant.Management
{
    public class ManagementConfiguration
    {
        [Required]
        public ulong GuildId { get; set; }
    }
}
