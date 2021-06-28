using System;
using System.Collections.Generic;
using System.Linq;

using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Objects;
using Remora.Discord.Core;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public static class UnknownEventDescriptorExtensions
    {
        public static IEmbed RenderInfo(this IReadOnlyList<UnknownEventDescriptor> descriptors)
        {
            var countsByVersion = new Dictionary<Version, int>(descriptors.Count);
            var oldestReceived = DateTimeOffset.MaxValue;
            var newestReceived = DateTimeOffset.MinValue;

            foreach (var descriptor in descriptors)
            {
                if (!countsByVersion.ContainsKey(descriptor.RemoraApiVersion))
                    countsByVersion[descriptor.RemoraApiVersion] = 1;
                else
                    ++countsByVersion[descriptor.RemoraApiVersion];

                if (descriptor.Received < oldestReceived)
                    oldestReceived = descriptor.Received;

                if (descriptor.Received > newestReceived)
                    newestReceived = descriptor.Received;
            }

            return new Embed(
                Title:          descriptors.Count switch
                {
                    0   => "There are no unknown Discord Gateway events available for analysis.",
                    1   => "There is 1 unknown Discord Gateway event available for analysis.",
                    _   => $"There are {descriptors.Count} unknown Discord Gateway events available for analysis."
                },
                Description:    (descriptors.Count is 0)
                    ? default(Optional<string>)
                    : "Use the \"/assistant unknown-events\" commands to download and clear the unknown events log.",
                Fields:         Enumerable.Empty<EmbedField>()
                    .Concat(countsByVersion
                        .Select(cbv => new EmbedField($"API Version {cbv.Key}", $"{cbv.Value} events")))
                    .Concat((descriptors.Count is 1)
                        ? new[]
                        {
                            new EmbedField("Event received", oldestReceived.ToString("R"))
                        }
                        : new[]
                        {
                            new EmbedField("Oldest event received", oldestReceived.ToString("R")),
                            new EmbedField("Newest event received", newestReceived.ToString("R"))
                        })
                    .ToArray(),
                Timestamp:      DateTimeOffset.UtcNow,
                Footer:         new EmbedFooter($"API Version {MonitorService.RemoraApiVersion}"));
        }
    }
}
