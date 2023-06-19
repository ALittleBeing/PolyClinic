using Microsoft.Extensions.Logging;

namespace PolyClinic.Common.Logger
{
    public class FileLogger : ILogger
    {
        protected readonly FileLoggerProvider _fileLoggerProvider;

        public FileLogger([System.Diagnostics.CodeAnalysis.NotNull] FileLoggerProvider fileLoggerProvider)
        {
            _fileLoggerProvider = fileLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var fullFilePath = _fileLoggerProvider.Options.FolderPath + "/" + _fileLoggerProvider.Options.FilePath.Replace("{date}", DateTimeOffset.Now.ToString("yyyyMMdd"));
            var logRecord = string.Format("{0} [{1}] {2} {3}", "[" + DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]", logLevel.ToString(), formatter(state, exception), exception != null ? exception.StackTrace : "");

            using (var streamWriter = new System.IO.StreamWriter(fullFilePath, true))
            {
                streamWriter.WriteLine(logRecord);
            }
        }

    }
}
