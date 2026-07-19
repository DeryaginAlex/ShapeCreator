using ShapeCreator.Services.Interface;
using System.IO;

namespace ShapeCreator.Services;

public class LoggerService : ILoggerService
{
    private readonly object _lock = new object();
    private string LogPath => Path.Combine("logs", $"{DateTime.Now:yyyy-MM-dd}.log");

    private void CreateLogFile()
    {
        string directory = Path.GetDirectoryName(LogPath) ?? string.Empty;

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(LogPath))
        {
            File.Create(LogPath).Close();
        }
    }

    private void Write(string level, string message)
    {
        try
        {
            CreateLogFile();

            lock (_lock)
            {
                File.AppendAllText(LogPath, $@"{DateTime.Now:HH:mm:ss} - [{level}] {message}{Environment.NewLine}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError($"Failed to write to log file: {ex.Message}");
        }
    }

    private void Write(string message)
    {
        try
        {
            CreateLogFile();

            lock (_lock)
            {
                File.AppendAllText(LogPath, $@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError($"Failed to write to log file: {ex.Message}");
        }
    }

    public void Separator()
    {
        Write(new string('=', 80));
    }

    public void Trace(string message)
    {
        Write("Trace", message);
    }

    public void Debug(string message)
    {
        Write("Debug", message);
    }

    public void Info(string message)
    {
        Write("Info", message);
    }

    public void Warn(string message)
    {
        Write("Warn", message);
    }

    public void Error(string message)
    {
        Write("Error", message);
    }

    public void Error(string message, Exception ex)
    {
        string fullMessage = $"{message}: {ex.Message}{Environment.NewLine}StackTrace: {ex.StackTrace}";
        Write("Error", fullMessage);
    }
}
