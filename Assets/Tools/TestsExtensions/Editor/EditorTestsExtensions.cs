using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tools.TestsExtensions.Editor
{
    public static class EditorTestsExtensions
    {
        public static IEnumerable<string> GetAllScenesPaths() =>
            AssetDatabase
                .FindAssets("t:Scene", new[] { "Assets" })
                .Select(AssetDatabase.GUIDToAssetPath);

        public static bool HasMissingComponents(this GameObject gameObject) => 
            GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject) > 0;
    }
}