using System;

namespace Microsoft.Extensions.Logging
{
    public static class EnumExtensions
    {
        public static EventId ToEventId<T>(this T @event)
                where T : IConvertible
            => new(
                id:     @event.ToInt32(null),
                name:   @event.ToString());
    }
}
