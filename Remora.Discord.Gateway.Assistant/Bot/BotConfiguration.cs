using System;
using System.ComponentModel.DataAnnotations;

namespace Remora.Discord.Gateway.Assistant.Bot
{
    public class BotConfiguration
    {
        [Required]
        public string Token { get; set; }
            = null!;

        [Required]
        public TimeSpan RestartInterval { get; set; }
    }
}
