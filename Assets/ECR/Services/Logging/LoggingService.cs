using System;
using UnityEngine;

using static System.Text.RegularExpressions.Regex;
using static ECR.Constants.Logging;

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
            string.Format(
                MESSAGE_TEMPLATE,
                GetHexColor(sender.GetType()),
                sender.GetType().Name,
                message
            );

        private static string GetHexColor(Type sender) =>
            sender.Namespace switch
            {
                var x when IsMatch(x, INFRASTRUCTURE_REGEX) => INFRASTRUCTURE_COLOR,
                var x when IsMatch(x, META_REGEX)           => META_COLOR,
                var x when IsMatch(x, GAMEPLAY_REGEX)       => GAMEPLAY_COLOR,
                _                                                             => DEFAULT_COLOR
            };
    }
}