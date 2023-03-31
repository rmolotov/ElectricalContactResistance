namespace ECR.Services.Logging
{
    public interface ILoggingService
    {
        void LogMessage(string message, object sender = null);
        void LogWarning(string message, object sender = null);
        void LogError(string message, object sender = null);
    }
}