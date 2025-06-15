using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools.TestsExtensions.Editor
{
    public static class EditorTestsExtensions
    {
        public static IEnumerable<Scene> GetAndOpenAllScenes()
        {
            var scenesPaths = AssetDatabase
                .FindAssets("t:Scene", new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath);

            foreach (var path in scenesPaths)
            {
                var sceneByPath = SceneManager.GetSceneByPath(path);
                if (sceneByPath.isLoaded)
                {
                    yield return sceneByPath;
                    continue;
                }

                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

                yield return scene;
                
                EditorSceneManager.CloseScene(scene, true);
            }
        }

        public static bool HasMissingComponents(this GameObject gameObject)
        {
            return GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject) > 0;
        }
    }
}