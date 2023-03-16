using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSG.Extensions
{ 
    public static class PromiseExtensions
    { 
        internal class PromiseMono : MonoBehaviour { }

        private static MonoBehaviour _routineBehaviour;
        private static Dictionary<Promise, Coroutine> _promiseDictionary = new();

        public static void WaitAbort(this Promise promise)
        {
            if( _promiseDictionary.TryGetValue(promise, out var routine))
            {
                _routineBehaviour.StopCoroutine(routine);
                _promiseDictionary.Remove(promise);
            }
            else
            {
                Debug.LogWarning("No routine found to abort");
            }
        }

        public static Promise WaitFor(this Promise promise, float seconds)
        {
            AssureInit();
            var promiseRoutine = _routineBehaviour.StartCoroutine(TimerRoutine(seconds, promise));
            _promiseDictionary[promise] = promiseRoutine;
            promise.Finally(() => _promiseDictionary.Remove(promise));
            return promise;
        }

        public static Promise WaitFor(this Promise promise, IEnumerator routine)
        {
            AssureInit();
            var promiseRoutine = _routineBehaviour.StartCoroutine(WaitForRoutine(routine, promise));
            _promiseDictionary[promise] = promiseRoutine;
            promise.Finally(() => _promiseDictionary.Remove(promise));
            return promise;
        }

        public static Promise WaitFor(this Promise promise, Coroutine routine)
        {
            AssureInit();
            var promiseRoutine = _routineBehaviour.StartCoroutine(WaitForRoutine(routine, promise));
            _promiseDictionary[promise] = promiseRoutine;
            promise.Finally(() => _promiseDictionary.Remove(promise));
            return promise;
        }

        public static Promise WaitFor(this Promise promise, Func<bool> predicate)
        {
            AssureInit();
            var promiseRoutine = _routineBehaviour.StartCoroutine(WaitForPredicate(predicate, promise));
            _promiseDictionary[promise] = promiseRoutine;
            promise.Finally(() => _promiseDictionary.Remove(promise));
            return promise;
        }

        public static Promise WaitForFrames(this Promise promise, int frames)
        {
            AssureInit();
            var promiseRoutine = _routineBehaviour.StartCoroutine(FramesRoutine(frames, promise));
            _promiseDictionary[promise] = promiseRoutine;
            promise.Finally(() => _promiseDictionary.Remove(promise));
            return promise;
        }

        public static void TryResolve(this IPendingPromise promise)
        {
            if (promise.IsPending()) promise.Resolve();
        }

        public static bool IsPending(this IPendingPromise promise) => 
            promise is Promise {CurState: PromiseState.Pending};

        public static bool IsPending<T>(this IPendingPromise<T> promise) => 
            promise is Promise<T> {CurState: PromiseState.Pending};

        public static void ResolveIfPending(this IPendingPromise promise)
        {
            if(promise.IsPending())
                promise.Resolve();
        }
        
        public static void ResolveIfPending<T>(this IPendingPromise<T> promise, T value)
        {
            if(promise.IsPending())
                promise.Resolve(value);
        }
        

        private static void AssureInit()
        {
            if( _routineBehaviour != null)
                return;

            var newObject = new GameObject("Promise Routine Runner");
            MonoBehaviour.DontDestroyOnLoad(newObject);
            newObject.hideFlags = HideFlags.HideInHierarchy;
            _routineBehaviour = newObject.AddComponent<PromiseMono>();
        }

        private static IEnumerator TimerRoutine(float seconds, Promise promise)
        {
            yield return new WaitForSecondsRealtime(seconds);
            promise.Resolve();
        }

        private static IEnumerator FramesRoutine(int frames, Promise promise)
        {
            for (var i = 0; i < frames; ++i)
                yield return new WaitForEndOfFrame();
            promise.Resolve();
        }

        private static IEnumerator WaitForRoutine(IEnumerator routine, Promise promise)
        {
            yield return routine;
            promise.Resolve();
        }

        private static IEnumerator WaitForRoutine(Coroutine routine, Promise promise)
        {
            yield return routine;
            promise.Resolve();
        }

        private static IEnumerator WaitForPredicate(Func<bool> predicate, Promise promise)
        {
            while(predicate.Invoke() == false)
                yield return null;

            promise.Resolve();
        }
    }
}