using System.Linq;
using NUnit.Framework;
using UnityEditor;

using static Tools.TestsExtensions.Editor.EditorTestsExtensions;
using static Tools.TestsExtensions.Common.CommonTestsExtensions;

namespace Tests.EditMode
{
    public class ValidationTests
    {
        private const string MissingComponentReportTemplate = "[{0}] on [{1}] scene has missing component(s)";
        private const string MissingPrefabComponentTemplate = "[{0}] on [{1}] scene is instance of missing prefab";

        [Test]
        public void FindMissingComponents()
        {
            var errors =
                from scene in GetAndOpenAllScenes()
                from gameObject in GetAllGameObjects(scene)
                where gameObject.HasMissingComponents()
                select string.Format(MissingComponentReportTemplate, gameObject.name, gameObject.scene.name);
            
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void FindMissingPrefabsInstances()
        {
            var errors =
                from scene in GetAndOpenAllScenes()
                from gameObject in GetAllGameObjects(scene)
                where PrefabUtility.IsPrefabAssetMissing(gameObject)
                select string.Format(MissingPrefabComponentTemplate, gameObject.name, gameObject.scene.name);
            
            Assert.That(errors, Is.Empty);
        }
    }
}

