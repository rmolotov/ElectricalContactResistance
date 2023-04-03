using UnityEngine;
using System;

#if (UNITY_ANDROID && !UNITY_EDITOR)
using System.Text;
using System.Runtime.InteropServices;
#elif (UNITY_IOS && !UNITY_EDITOR)
using UnityEngine.iOS;
using System.Runtime.InteropServices;
#endif

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// C# wrapper for the Lofelt Studio Android and iOS SDK.
    /// </summary>
    ///
    /// You should not use this class directly, use HapticController instead, or the
    /// <c>MonoBehaviour</c> classes HapticReceiver and HapticSource.
    ///
    /// The Lofelt Studio Android and iOS SDK are included in Nice Vibrations as pre-compiled
    /// binary plugins.
    ///
    /// Each method here delegates to either the Android or iOS SDK. The methods should only be
    /// called if DeviceMeetsMinimumPlatformRequirements() returns true, otherwise there will
    /// be runtime errors.
    ///
    /// All the methods do nothing when running in the Unity editor.
    ///
    /// Before calling any other method, Initialize() needs to be called.
    ///
    /// Errors are printed and swallowed, no exceptions are thrown. On iOS, this happens inside
    /// the SDK, on Android this happens with try/catch blocks in this class.
    public static class LofeltHaptics
    {
#if (UNITY_ANDROID && !UNITY_EDITOR)
        static AndroidJavaObject lofeltHaptics;
        static AndroidJavaObject hapticPatterns;
        static long nativeController;

        [DllImport("lofelt_sdk")]
        private static extern bool lofeltHapticsLoadDirect(IntPtr controller, [In] byte[] bytes, long size);

#elif (UNITY_IOS && !UNITY_EDITOR)
        // imports of iOS Framework bindings

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsDeviceMeetsMinimumRequirementsBinding();

        [DllImport("__Internal")]
        private static extern IntPtr lofeltHapticsInitBinding();

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsLoadBinding(IntPtr controller, [In] byte[] bytes, long size);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsPlayBinding(IntPtr controller);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsStopBinding(IntPtr controller);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSeekBinding(IntPtr controller, float time);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSetAmplitudeMultiplicationBinding(IntPtr controller, float factor);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSetFrequencyShiftBinding(IntPtr controller, float shift);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsLoopBinding(IntPtr controller, bool enable);

        [DllImport("__Internal")]
        private static extern float lofeltHapticsGetClipDurationBinding(IntPtr controller);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsReleaseBinding(IntPtr controller);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSystemHapticsTriggerBinding(int type);

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSystemHapticsInitializeBinding();

        [DllImport("__Internal")]
        private static extern bool lofeltHapticsSystemHapticsReleaseBinding();

        static IntPtr controller = IntPtr.Zero;

        static bool systemHapticsInitialized = false;
#endif

        /// <summary>
        /// Initializes the iOS framework or Android library plugin.
        /// </summary>
        ///
        /// This needs to be called before calling any other method.
        public static void Initialize()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var context = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    lofeltHaptics = new AndroidJavaObject("com.lofelt.haptics.LofeltHaptics", context);
                    nativeController = lofeltHaptics.Call<long>("getControllerHandle");
                    hapticPatterns =  new AndroidJavaObject("com.lofelt.haptics.HapticPatterns", context);
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsSystemHapticsInitializeBinding();
            systemHapticsInitialized = true;
            controller = lofeltHapticsInitBinding();
#endif
        }

        /// <summary>
        /// Releases the resources used by the iOS framework or Android library plugin.
        /// </summary>
        public static void Release()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Dispose();
                lofeltHaptics = null;

                hapticPatterns.Dispose();
                hapticPatterns = null;
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            if(DeviceCapabilities.isVersionSupported) {
                lofeltHapticsSystemHapticsReleaseBinding();
                if(controller != IntPtr.Zero) {
                    lofeltHapticsReleaseBinding(controller);
                    controller = IntPtr.Zero;
                }
            }
#endif
        }

        public static bool DeviceMeetsMinimumPlatformRequirements()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                return lofeltHaptics.Call<bool>("deviceMeetsMinimumRequirements");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                return false;
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            return lofeltHapticsDeviceMeetsMinimumRequirementsBinding();
#else
            return true;
#endif
        }

        public static void Load(byte[] data)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                // For performance reasons, we do *not* call into the Java API with
                // `lofeltHaptics.Call("load", data)` here. Instead, we bypass the Java layer and
                // call into the native library directly, saving the costly conversion from
                // C#'s byte[] to Java's byte[].
                lofeltHapticsLoadDirect((IntPtr)nativeController, data, data.Length);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsLoadBinding(controller, data, data.Length);
#endif
        }

        public static float GetClipDuration()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                return lofeltHaptics.Call<float>("getClipDuration");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                return 0.0f;
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            return lofeltHapticsGetClipDurationBinding(controller);
#else
            //No haptic clip was loaded with Lofelt SDK, so it returns 0.0f
            return 0.0f;
#endif
        }

        public static void Play()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("play");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsPlayBinding(controller);
#endif
        }

        public static void PlayMaximumAmplitudePattern(float[] timings)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                hapticPatterns.Call("playMaximumAmplitudePattern",timings);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#endif
        }

        public static void Stop()
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("stop");
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsStopBinding(controller);
#endif
        }

        public static void Seek(float time)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("seek", time);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsSeekBinding(controller, time);
#endif
        }

        public static void SetAmplitudeMultiplication(float factor)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("setAmplitudeMultiplication", factor);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsSetAmplitudeMultiplicationBinding(controller, factor);
#endif
        }

        public static void SetFrequencyShift(float shift)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsSetFrequencyShiftBinding(controller, shift);
#endif
        }

        public static void Loop(bool enabled)
        {
#if (UNITY_ANDROID && !UNITY_EDITOR)
            try
            {
                lofeltHaptics.Call("loop", enabled);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
            }
#elif (UNITY_IOS && !UNITY_EDITOR)
            lofeltHapticsLoopBinding(controller, enabled);
#endif
        }

        public static void TriggerPresetHaptics(int type)
        {
#if (UNITY_IOS && !UNITY_EDITOR)
            if (!systemHapticsInitialized)
            {
                lofeltHapticsSystemHapticsInitializeBinding();
                systemHapticsInitialized = true;
            }
            lofeltHapticsSystemHapticsTriggerBinding(type);
#endif
        }
    }
}
