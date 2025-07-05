using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using Tools.TestsExtensions.Common;
using Tools.TestsExtensions.Editor;

namespace Tests.EditMode
{
    public class ValidationTests
    {
        [TestCaseSource(
            typeof(EditorTestsExtensions),
            nameof(EditorTestsExtensions.GetAllScenesPaths)
        )]
        public void NoMissingComponents(string scenePath)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            var gameObjectsWithMissingComponents = CommonTestsExtensions.GetAllGameObjects(scene)
                .Where(gameObject => gameObject.HasMissingComponents())
                .Select(gameObject => gameObject.name)
                .ToList();

            EditorSceneManager.CloseScene(scene, true);

            gameObjectsWithMissingComponents
                .Should()
                .BeEmpty();
        }

        [TestCaseSource(
            typeof(EditorTestsExtensions),
            nameof(EditorTestsExtensions.GetAllScenesPaths)
        )]
        public void NoMissingPrefabsInstances(string scenePath)
        {
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

            var instancesOfMissingPrefabs = CommonTestsExtensions.GetAllGameObjects(scene)
                .Where(PrefabUtility.IsPrefabAssetMissing)
                .Select(gameObject => gameObject.name)
                .ToList();

            EditorSceneManager.CloseScene(scene, true);

            instancesOfMissingPrefabs
                .Should()
                .BeEmpty();
        }
    }
}

