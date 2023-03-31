using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ECR.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        public void LogMessage(string message, object sender = null) =>
            Debug.Log(GetString(message, sender ?? this));

        public void LogWarning(string message, object sender = null) =>
            Debug.LogWarning(GetString(message, sender ?? this));

        public void LogError(string message, object sender = null) =>
            Debug.LogError(GetString(message, sender ?? this));

        private static string GetString(string message, object sender) =>
            $"<b><i><color={GetHexColor(sender.GetType())}>{sender.GetType().Name}: </color></i></b> {message}";

        private static string GetHexColor(Type sender) =>
            sender.Namespace switch
            {
                var x when Regex.IsMatch(x, @".*Infrastructure.*") => "#e38d46",
                var x when Regex.IsMatch(x, @".*Meta.*")           => "#e346b4",
                var x when Regex.IsMatch(x, @".*Gameplay.*")       => "#4697e3",
                _                                                  => "#e3e3e3"
            };
    }
}