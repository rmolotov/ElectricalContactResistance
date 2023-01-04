using System;

namespace ECR.Infrastructure.SceneManagement
{
    public static class SceneManagementExtensions
    {
        public static SceneName ToSceneDestination(this string sceneName)
        {
            return sceneName switch
            {
                "Bootstrap" => SceneName.Bootstrap,
                "Meta" => SceneName.Meta,
                "Core" => SceneName.Core,
                _ => throw new ArgumentOutOfRangeException(nameof(sceneName), sceneName, null)
            };
        }

        public static bool IsGamePlayScene(this SceneName sceneName) =>
            sceneName
                is SceneName.Core;
    }
}