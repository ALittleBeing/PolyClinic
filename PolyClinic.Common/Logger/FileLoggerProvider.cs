using Microsoft.Extensions.Logging;

namespace PolyClinic.Common.Logger
{
    [ProviderAlias("PolyClinicLogFile")]
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly FileLoggerOptions Options;

        public FileLoggerProvider(Microsoft.Extensions.Options.IOptions<FileLoggerOptions> options)
        {
            Options = options.Value;

            if (!System.IO.Directory.Exists(Options.FolderPath))
            {
                System.IO.Directory.CreateDirectory(Options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
