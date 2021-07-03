using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.Gateway.Responders;
using Remora.Results;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public class MonitorResponder
        : IResponder<IUnknownEvent>
    {
        public MonitorResponder(
            ILogger<MonitorResponder>   logger,
            MonitorService              monitor)
        {
            _logger         = logger;
            _monitorService = monitor;
        }

        public async Task<Result> RespondAsync(IUnknownEvent gatewayEvent, CancellationToken ct = default)
        {
            MonitorLogger.UnknownEventReceived(_logger);
            await _monitorService.RecordAsync(gatewayEvent, ct);
            MonitorLogger.UnknownEventRecorded(_logger);

            return Result.FromSuccess();
        }

        private readonly ILogger        _logger;
        private readonly MonitorService _monitorService;
    }
}
