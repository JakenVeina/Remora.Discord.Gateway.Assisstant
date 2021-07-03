using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        public MonitorService(
            ILogger<MonitorService>         logger,
            IOptions<MonitorConfiguration>  monitorBotConfiguration)
        {
            _eventReceipts              = Channel.CreateUnbounded<EventReceipt>(new()
            {
                SingleReader = true,
                SingleWriter = true
            });
            _logger                     = logger;
            _monitorConfiguration       = monitorBotConfiguration;
            _unknownEventSavedSource    = new();
        }

        public void ClearUnknownEvents()
        {
            var unknownEventsLogPath = _monitorConfiguration.Value.UnknownEventsLogPath;

            MonitorLogger.UnknownEventsDeleting(_logger, unknownEventsLogPath);
            
            foreach(var filePath in Directory.EnumerateFiles(unknownEventsLogPath))
            {
                MonitorLogger.UnknownEventLogFileDeleting(_logger, filePath);
                File.Delete(filePath);
                MonitorLogger.UnknownEventLogFileDeleting(_logger, filePath);
            }

            MonitorLogger.UnknownEventsDeleted(_logger, unknownEventsLogPath);
        }

        public async Task<bool> TryArchiveUnknownEventsTo(Stream stream, CancellationToken cancellationToken)
        {
            MonitorLogger.UnknownEventsArchiving(_logger);

            var unknownEventsLogPath = _monitorConfiguration.Value.UnknownEventsLogPath;

            if (!Directory.Exists(unknownEventsLogPath))
            {
                MonitorLogger.UnknownEventsLogPathNotFound(_logger, unknownEventsLogPath);
                return false;
            }

            var filePaths = Directory.EnumerateFiles(unknownEventsLogPath)
                .ToArray();

            if (filePaths.Length == 0)
            {
                MonitorLogger.UnknownEventsLogPathEmpty(_logger, unknownEventsLogPath);
                return false;
            }

            MonitorLogger.UnknownEventsLogFilesFound(_logger, unknownEventsLogPath, filePaths.Length);

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);

            foreach(var filePath in filePaths)
            {
                MonitorLogger.UnknownEventLogFileArchiving(_logger, filePath);

                using var fileStream = File.OpenRead(filePath);

                var entry = archive.CreateEntry(filePath, CompressionLevel.Optimal);

                using (var entryStream = entry.Open())
                await fileStream.CopyToAsync(entryStream, cancellationToken);

                MonitorLogger.UnknownEventLogFileArchived(_logger, entry);
            }

            MonitorLogger.UnknownEventsArchived(_logger, filePaths.Length);
            return true;
        }

        public IEnumerable<UnknownEventDescriptor> EnumerateUnknownEvents()
        {
            var unknownEventsLogPath = _monitorConfiguration.Value.UnknownEventsLogPath;

            MonitorLogger.UnknownEventsEnumerating(_logger, unknownEventsLogPath);

            if (!Directory.Exists(unknownEventsLogPath))
            {
                MonitorLogger.UnknownEventsLogPathNotFound(_logger, unknownEventsLogPath);
                return Enumerable.Empty<UnknownEventDescriptor>();
            }

            return Directory.EnumerateFiles(unknownEventsLogPath)
                .Select(fn =>
                {
                    MonitorLogger.UnknownEventLogFileParsing(_logger, fn);
                    var isValid = UnknownEventDescriptor.TryParse(Path.GetFileNameWithoutExtension(fn), out var descriptor);
                    if (isValid)
                        MonitorLogger.UnknownEventLogFileParsed(_logger, descriptor);
                    else
                        MonitorLogger.UnknownEventLogFileParseFailed(_logger, fn);

                    return (isValid, descriptor);
                })
                .Where(x => x.isValid)
                .Select(x => x.descriptor);
        }

        public async ValueTask RecordAsync(IUnknownEvent gatewayEvent, CancellationToken cancellationToken)
        {
            MonitorLogger.UnknownEventRecording(_logger);
            await _eventReceipts.Writer.WriteAsync(
                new()
                {
                    Event = gatewayEvent,
                    Received = DateTimeOffset.UtcNow
                },
                cancellationToken);
            MonitorLogger.UnknownEventRecorded(_logger);
        }

        public Task WhenUnknownEventSavedAsync(CancellationToken cancellationToken)
            => Task.WhenAny(
                Task.Delay(Timeout.Infinite, cancellationToken),
                _unknownEventSavedSource.Task);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MonitorLogger.MonitorStarting(_logger);
            try
            {
                await foreach (var receipt in _eventReceipts.Reader.ReadAllAsync(stoppingToken))
                {
                    var unknownEventsLogPath = _monitorConfiguration.Value.UnknownEventsLogPath;

                    if (!Directory.Exists(unknownEventsLogPath))
                    {
                        MonitorLogger.UnknownEventsLogPathNotFound(_logger, unknownEventsLogPath);
                        Directory.CreateDirectory(unknownEventsLogPath);
                    }

                    var descriptor = new UnknownEventDescriptor()
                    {
                        Received            = receipt.Received,
                        RemoraApiVersion    = RemoraApiVersion
                    };
                    var filePath = Path.Combine(unknownEventsLogPath, $"{descriptor}.json");

                    MonitorLogger.UnknownEventLogFileSaving(_logger, filePath);
                    await File.WriteAllTextAsync(filePath, receipt.Event.Data, stoppingToken);
                    MonitorLogger.UnknownEventLogFileSaved(_logger, filePath);

                    Interlocked.Exchange(ref _unknownEventSavedSource, new())
                        .SetResult();
                }
            }
            finally
            {
                MonitorLogger.MonitorStopped(_logger);
            }
        }

        private readonly Channel<EventReceipt>          _eventReceipts;
        private readonly ILogger                        _logger;
        private readonly IOptions<MonitorConfiguration> _monitorConfiguration;
        
        private TaskCompletionSource _unknownEventSavedSource;

        private struct EventReceipt
        {
            public IUnknownEvent    Event;
            public DateTimeOffset   Received;
        }
    }
}
