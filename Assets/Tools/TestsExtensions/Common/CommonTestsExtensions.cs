using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools.TestsExtensions.Common
{
    public static class CommonTestsExtensions
    {
        public static IEnumerable<GameObject> GetAllGameObjects(Scene scene)
        {
            var queue = new Queue<GameObject>(scene.GetRootGameObjects());

            while (queue.Count > 0)
            {
                var gameObject = queue.Dequeue();
                
                yield return gameObject;
                
                foreach (Transform child in gameObject.transform) 
                    queue.Enqueue(child.gameObject);
            }
        }
    }
}