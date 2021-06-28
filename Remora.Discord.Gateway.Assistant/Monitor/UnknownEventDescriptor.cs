using System;
using System.Globalization;

namespace Remora.Discord.Gateway.Assistant.Monitor
{
    public struct UnknownEventDescriptor
    {
        public static bool TryParse(string fileName, out UnknownEventDescriptor descriptor)
        {
            descriptor = default;

            var pieces = fileName.Split(_separator);
            if (pieces.Length is not 2)
                return false;

            if (!DateTimeOffset.TryParseExact(pieces[0], _receivedFormat, null, DateTimeStyles.None, out var received))
                return false;

            if (!Version.TryParse(pieces[1], out var remoraApiVersion))
                return false;

            descriptor = new()
            {
                Received            = received,
                RemoraApiVersion    = remoraApiVersion
            };

            return true;
        }

        public DateTimeOffset Received { get; init; }

        public Version RemoraApiVersion { get; init; }

        public override string ToString()
            => $"{Received.ToString(_receivedFormat)}{_separator}{RemoraApiVersion}";

        private const string _receivedFormat
            = "yyyy-MM-ddTHH-mm-ssZ";

        private const char _separator
            = '_';
    }
}
