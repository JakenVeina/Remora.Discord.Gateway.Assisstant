using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Remora.Discord.API.Abstractions.Gateway.Events;
using Remora.Discord.API.Gateway.Events;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public class MonitorService
        : BackgroundService
    {
        public static readonly Version RemoraApiVersion
            = typeof(UnknownEvent).Assembly.GetName().Version!;


        public MonitorService(IOptions<MonitorConfiguration> monitorBotConfiguration)
        {
            _eventReceipts          = Channel.CreateUnbounded<EventReceipt>(new()
            {
                SingleReader = true,
                SingleWriter = true
            });
            _monitorConfiguration   = monitorBotConfiguration;
            _recordedSource         = new();
        }

        public void ClearUnknownEvents()
            => Directory.Delete(_monitorConfiguration.Value.EventsLogPath);

        public async Task<bool> TryWriteUnknownEventsTo(Stream stream, CancellationToken cancellationToken)
        {
            if (!Directory.Exists(_monitorConfiguration.Value.EventsLogPath))
                return false;

            var filePaths = Directory.EnumerateFiles(_monitorConfiguration.Value.EventsLogPath)
                .ToArray();

            if (filePaths.Length == 0)
                return false;

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);

            foreach(var filePath in filePaths)
            {
                using var fileStream = File.OpenRead(filePath);

                var entry = archive.CreateEntry(filePath, CompressionLevel.Optimal);
                using var entryStream = entry.Open();

                await fileStream.CopyToAsync(entryStream, cancellationToken);
            }

            return true;
        }

        public IEnumerable<UnknownEventDescriptor> EnumerateUnknownEvents()
            => Directory.Exists(_monitorConfiguration.Value.EventsLogPath)
                ? Directory.EnumerateFiles(_monitorConfiguration.Value.EventsLogPath)
                    .Select(fn => (isValid: UnknownEventDescriptor.TryParse(Path.GetFileNameWithoutExtension(fn), out var descriptor), descriptor))
                    .Where(x => x.isValid)
                    .Select(x => x.descriptor)
                : Enumerable.Empty<UnknownEventDescriptor>();

        public ValueTask RecordAsync(IUnknownEvent gatewayEvent, CancellationToken cancellationToken)
            => _eventReceipts.Writer.WriteAsync(
                new()
                {
                    Event = gatewayEvent,
                    Received = DateTimeOffset.UtcNow
                },
                cancellationToken);

        public Task WhenRecordedAsync(CancellationToken cancellationToken)
            => Task.WhenAny(
                Task.Delay(Timeout.Infinite, cancellationToken),
                _recordedSource.Task);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var receipt in _eventReceipts.Reader.ReadAllAsync(stoppingToken))
            {
                var eventsLogPath = _monitorConfiguration.Value.EventsLogPath;
                if (!Directory.Exists(eventsLogPath))
                    Directory.CreateDirectory(eventsLogPath);

                var descriptor = new UnknownEventDescriptor()
                {
                    Received            = receipt.Received,
                    RemoraApiVersion    = RemoraApiVersion
                };

                await File.WriteAllTextAsync(Path.Combine(eventsLogPath, $"{descriptor}.json"), receipt.Event.Data, stoppingToken);

                Interlocked.Exchange(ref _recordedSource, new())
                    .SetResult();
            }
        }

        private readonly Channel<EventReceipt>          _eventReceipts;
        private readonly IOptions<MonitorConfiguration> _monitorConfiguration;
        
        private TaskCompletionSource _recordedSource;

        private struct EventReceipt
        {
            public IUnknownEvent    Event;
            public DateTimeOffset   Received;
        }
    }
}
