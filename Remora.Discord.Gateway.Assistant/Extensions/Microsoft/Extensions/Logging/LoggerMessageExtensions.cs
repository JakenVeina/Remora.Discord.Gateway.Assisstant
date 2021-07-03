using System;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerMessageExtensions
    {
        public static Action<ILogger> WithoutException(this Action<ILogger, Exception?> loggerMessage)
            => logger => loggerMessage.Invoke(logger, null);

        public static Action<ILogger, T1> WithoutException<T1>(this Action<ILogger, T1, Exception?> loggerMessage)
            => (logger, value1) => loggerMessage.Invoke(logger, value1, null);

        public static Action<ILogger, T1, T2> WithoutException<T1, T2>(this Action<ILogger, T1, T2, Exception?> loggerMessage)
            => (logger, value1, value2) => loggerMessage.Invoke(logger, value1, value2, null);

        public static Action<ILogger, T1, T2, T3> WithoutException<T1, T2, T3>(this Action<ILogger, T1, T2, T3, Exception?> loggerMessage)
            => (logger, value1, value2, value3) => loggerMessage.Invoke(logger, value1, value2, value3, null);
    }
}
