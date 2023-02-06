using System;

namespace ECR.Infrastructure.SceneManagement
{
    public static class SceneManagementExtensions
    {
        public static SceneName ToSceneName(this string sceneName)
        {
            return sceneName switch
            {
                "Bootstrap" => SceneName.Bootstrap,
                "Meta"      => SceneName.Meta,
                "Core"      => SceneName.Core,
                _           => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }

        public static string ToSceneString(this SceneName sceneName)
        {
            return sceneName switch
            {
                SceneName.Bootstrap => "Bootstrap",
                SceneName.Meta      => "Meta",
                SceneName.Core      => "Core",
                _                   => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }

        public static bool IsGamePlayScene(this SceneName sceneName) =>
            sceneName
                is SceneName.Core;
    }
}