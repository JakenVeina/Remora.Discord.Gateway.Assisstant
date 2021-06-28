using System.Threading;
using System.Threading.Tasks;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public class MonitorResponder
        : IResponder<IUnknownEvent>
    {
        public MonitorResponder(MonitorService monitor)
            => _monitorService = monitor;

        public async Task<Result> RespondAsync(IUnknownEvent gatewayEvent, CancellationToken ct = default)
        {
            await _monitorService.RecordAsync(gatewayEvent, ct);

            return Result.FromSuccess();
        }

        private readonly MonitorService _monitorService;
    }
}
