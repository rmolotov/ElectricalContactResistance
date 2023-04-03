using System;
using UnityEngine;
using System.Globalization;

namespace Lofelt.NiceVibrations
{
    /// <summary>
    /// A collection of methods to play simple haptic patterns.
    /// </summary>
    ///
    /// Each of the methods here load and play a simple haptic clip. After playback
    /// has finished, the clip will remain loaded in HapticController.
    ///
    /// The only method that will not call HapticController to load a haptic clip is
    /// PlayPreset(), when called on iOS.
    public static class HapticPatterns
    {
        static String emphasisTemplate;
        static String constantTemplate;
        static String patternTemplate;
        static NumberFormatInfo numberFormat;

        /// <summary>
        /// Enum that represents all the types of haptic presets available
        /// </summary>
        public enum PresetType
        {
            Selection = 0,
            Success = 1,
            Warning = 2,
            Failure = 3,
            LightImpact = 4,
            MediumImpact = 5,
            HeavyImpact = 6,
            RigidImpact = 7,
            SoftImpact = 8,
            None = -1
        }

        /// <summary>
        /// Structure that represents a haptic pattern with amplitude variations.
        /// </summary>
        ///
        /// Different amplitude values in time are automatically interpolated by the device.
        ///
        /// \ref time values have be incremental to be played using PlayPattern().
        struct Pattern
        {
            internal float[] time;
            internal float[] amplitude;
            internal String clip;
            internal Pattern(float[] time, float[] amplitude)
            {
                this.time = time;
                this.amplitude = amplitude;
                this.clip = "";
            }
        }


        /// <summary>
        /// Predefined Pattern that represents a "Selection" haptic preset
        /// </summary>
        static Pattern Selection =
            new Pattern(new float[] { 0.0f, 0.01f },
                                 new float[] { 0.471f, 0.471f });
        /// <summary>
        /// Predefined Pattern that represents a "Light" haptic preset
        /// </summary>
        static Pattern Light =
            new Pattern(new float[] { 0.0f, 0.02f },
                                 new float[] { 0.156f, 0.156f });
        /// <summary>
        /// Predefined Pattern that represents a "Medium" haptic preset
        /// </summary>
        static Pattern Medium =
            new Pattern(new float[] { 0.0f, 0.04f },
                                 new float[] { 0.471f, 0.471f });
        /// <summary>
        /// Predefined Pattern that represents a "Heavy" haptic preset
        /// </summary>
        static Pattern Heavy =
            new Pattern(new float[] { 0.0f, 0.08f },
                                 new float[] { 1.0f, 1.0f });
        /// <summary>
        /// Predefined Pattern that represents a "Rigid" haptic preset
        /// </summary>
        static Pattern Rigid =
            new Pattern(new float[] { 0.0f, 0.02f },
                                 new float[] { 1.0f, 1.0f });
        /// <summary>
        /// Predefined Pattern that represents a "Soft" haptic preset
        /// </summary>
        static Pattern Soft =
            new Pattern(new float[] { 0.0f, 0.08f },
                                 new float[] { 0.156f, 0.156f });
        /// <summary>
        /// Predefined Pattern that represents a "Success" haptic preset
        /// </summary>
        static Pattern Success =
            new Pattern(new float[] { 0.0f, 0.020f, 0.040f, 0.120f },
                                 new float[] { 0.0f, 0.157f, 0.0f, 1.0f });
        /// <summary>
        /// Predefined Pattern that represents a "Failure" haptic preset
        /// </summary>
        static Pattern Failure =
            new Pattern(new float[] { 0.0f, 0.040f, 0.060f, 0.100f, 0.120f, 0.200f, 0.220f, 0.240f },
                                 new float[] { 0.0f, 0.47f, 0.0f, 0.47f, 0.0f, 1.0f, 0.0f, 0.157f });
        /// <summary>
        /// Predefined Pattern that represents a "Warning" haptic preset
        /// </summary>
        static Pattern Warning =
            new Pattern(new float[] { 0.0f, 0.060f, 0.120f, 0.140f },
                                 new float[] { 0.0f, 1.0f, 0.0f, 0.47f });

        static HapticPatterns()
        {
            emphasisTemplate = (Resources.Load("nv-emphasis-template") as TextAsset).text;
            constantTemplate = (Resources.Load("nv-constant-template") as TextAsset).text;
            patternTemplate = (Resources.Load("nv-pattern-template") as TextAsset).text;

            numberFormat = new NumberFormatInfo();
            numberFormat.NumberDecimalSeparator = ".";
        }

        /// <summary>
        /// Plays a single emphasis point.
        /// </summary>
        ///
        /// Plays a haptic clip that consists only of one breakpoint with emphasis.
        /// On iOS, this translates to a transient, and on Android and gamepads to
        /// a quick vibration.
        ///
        /// <param name="amplitude">The amplitude of the emphasis, from 0.0 to 1.0</param>
        /// <param name="frequency">The frequency of the emphasis, from 0.0 to 1.0</param>
        public static void PlayEmphasis(float amplitude, float frequency)
        {
            if (emphasisTemplate == null)
            {
                return;
            }

            float clampedAmplitude = Mathf.Clamp(amplitude, 0.0f, 1.0f);
            float clampedFrequency = Mathf.Clamp(frequency, 0.0f, 1.0f);
            const float duration = 0.1f;

            String json = emphasisTemplate
                .Replace("{amplitude}", clampedAmplitude.ToString(numberFormat))
                .Replace("{frequency}", clampedFrequency.ToString(numberFormat))
                .Replace("{duration}", duration.ToString(numberFormat));

            GamepadRumble rumble = new GamepadRumble();
            rumble.durationsMs = new int[] { (int)(duration * 1000) };
            rumble.lowFrequencyMotorSpeeds = new float[] { clampedAmplitude };
            rumble.highFrequencyMotorSpeeds = new float[] { clampedFrequency };

            HapticController.Load(System.Text.Encoding.UTF8.GetBytes(json), rumble);
            HapticController.Loop(false);
            SetEmphasisFallbackPresetsFromAmplitude(amplitude);
            HapticController.Play();
        }

        /// <summary>
        /// Automatically selects the fallback preset based on the emphasis point amplitude.
        /// </summary>
        ///
        /// <param name="amplitude">The amplitude of the emphasis, from 0.0 to 1.0</param>
        static void SetEmphasisFallbackPresetsFromAmplitude(float amplitude)
        {
            if (amplitude > 0.5f)
            {
                HapticController.fallbackPreset = HapticPatterns.PresetType.HeavyImpact;
            }
            else if (amplitude <= 0.5f && amplitude > 0.3)
            {
                HapticController.fallbackPreset = HapticPatterns.PresetType.MediumImpact;
            }
            else if (amplitude <= 0.3f)
            {
                HapticController.fallbackPreset = HapticPatterns.PresetType.LightImpact;
            }
        }

        /// <summary>
        /// Plays a haptic with constant amplitude and frequency.
        /// </summary>
        ///
        /// On iOS and with gamepads, you can use HapticController::clipLevel to modulate the haptic
        /// while it is playing. iOS additional supports modulating the frequency with
        /// HapticController::clipFrequencyShift.
        ///
        /// <param name="amplitude">Amplitude, from 0.0 to 1.0</param>
        /// <param name="frequency">Frequency, from 0.0 to 1.0</param>
        /// <param name="duration">Play duration in seconds</param>
        public static void PlayConstant(float amplitude, float frequency, float duration)
        {
            if (constantTemplate == null)
            {
                return;
            }

            float clampedAmplitude = Mathf.Clamp(amplitude, 0.0f, 1.0f);
            float clampedFrequency = Mathf.Clamp(frequency, 0.0f, 1.0f);
            float clampedDurationSecs = Mathf.Max(duration, 0.0f);

            String json = constantTemplate
                .Replace("{duration}", clampedDurationSecs.ToString(numberFormat));

            GamepadRumble rumble = new GamepadRumble();
            int rumbleDurationMs = (int)(clampedDurationSecs * 1000);
            const int rumbleEntryDurationMs = 16; // One rumble entry per frame at 60 FPS, which is the limit of what GamepadRumbler can play
            int rumbleEntryCount = rumbleDurationMs / rumbleEntryDurationMs;
            rumble.durationsMs = new int[rumbleEntryCount];
            rumble.lowFrequencyMotorSpeeds = new float[rumbleEntryCount];
            rumble.highFrequencyMotorSpeeds = new float[rumbleEntryCount];

            // Create many rumble entries instead of just one. With just one entry, changing
            // clipLevel while the rumble is playing would have no effect, as GamepadRumbler applies
            // a change only to the next rumble entry, not the one currently playing.
            for (int i = 0; i < rumbleEntryCount; i++)
            {
                rumble.durationsMs[i] = rumbleEntryDurationMs;
                rumble.lowFrequencyMotorSpeeds[i] = 1.0f;
                rumble.highFrequencyMotorSpeeds[i] = 1.0f;
            }

            HapticController.Load(System.Text.Encoding.UTF8.GetBytes(json), rumble);
            HapticController.Loop(false);
            HapticController.clipLevel = clampedAmplitude;
            HapticController.clipFrequencyShift = clampedFrequency;
            HapticController.Play();
        }

        /// <summary>
        /// Plays an amplitude pattern defined in a \ref Pattern structure
        /// </summary>
        ///
        /// Won't play in case the amplitude and time arrays have different lengths.
        ///
        /// On Android, in case the device doesn't support minimum requirements, the pattern will be
        /// played with the same amplitude by turning the motor off and on for the duration of
        /// every 2 adjacent points in Pattern.time. On iOS, it won't do anything.
        ///
        /// <param name="pattern">\ref Pattern struct to be played</param>
        private static void PlayPattern(Pattern pattern)
        {
            if (patternTemplate == null)
            {
                return;
            }

            if (pattern.time.Length == 0 ||
                pattern.amplitude.Length != pattern.time.Length)
            {
                return;
            }

            if (HapticController.Init())
            {
                String amplitudeEnvelope = "";
                for (int i = 0; i < pattern.time.Length; i++)
                {
                    float clampedAmplitude = Mathf.Clamp(pattern.amplitude[i], 0.0f, 1.0f);
                    amplitudeEnvelope += "{ \"time\":" + pattern.time[i] + "," +
                                            "\"amplitude\":" + clampedAmplitude + "}";

                    // Don't add a comma to the JSON data if we're at the end of the envelope
                    if (i + 1 < pattern.time.Length)
                    {
                        amplitudeEnvelope += ",";
                    }
                }

                pattern.clip = patternTemplate
                    .Replace("{amplitude-envelope}", amplitudeEnvelope);

                HapticController.Load(System.Text.Encoding.UTF8.GetBytes(pattern.clip));
                HapticController.Loop(false);
                HapticController.Play();
            }
            else if (DeviceCapabilities.isVersionSupported)
            {
                LofeltHaptics.PlayMaximumAmplitudePattern(pattern.time);
            }
        }

        // Returns the Pattern associated with the given preset
        private static Pattern PatternForPreset(PresetType presetType)
        {
            switch (presetType)
            {
                case PresetType.Selection:
                    return Selection;
                case PresetType.LightImpact:
                    return Light;
                case PresetType.MediumImpact:
                    return Medium;
                case PresetType.HeavyImpact:
                    return Heavy;
                case PresetType.RigidImpact:
                    return Rigid;
                case PresetType.SoftImpact:
                    return Soft;
                case PresetType.Success:
                    return Success;
                case PresetType.Failure:
                    return Failure;
                case PresetType.Warning:
                    return Warning;
            }

            // Silence compiler warning about not all code paths returning something
            return Medium;
        }

        // Converts a Pattern to a GamepadRumble
        //
        // Each pair of adjacent entries in the Pattern create one entry in the GamepadRumble.
        private static GamepadRumble PatternToRumble(Pattern pattern)
        {
            GamepadRumble result = new GamepadRumble();
            if (pattern.time.Length <= 1)
            {
                return result;
            }

            Debug.Assert(pattern.time.Length == pattern.amplitude.Length);

            // The first pattern entry needs to have a time of 0.0 for the algorithm below to work
            Debug.Assert(pattern.time[0] == 0.0f);

            int rumbleCount = pattern.time.Length - 1;
            result.durationsMs = new int[rumbleCount];
            result.lowFrequencyMotorSpeeds = new float[rumbleCount];
            result.highFrequencyMotorSpeeds = new float[rumbleCount];
            result.totalDurationMs = 0;
            for (int rumbleIndex = 0; rumbleIndex < rumbleCount; rumbleIndex++)
            {
                int patternDurationMs = (int)((pattern.time[rumbleIndex + 1] - pattern.time[rumbleIndex]) * 1000.0f);

                // Rumble for twice as long as the actual pattern duration. Otherwise rumble entries
                // would be too short to be played back properly. Short entries cause problems for
                // two reasons:
                // 1. The timer resolution of GamepadRumbler is limited to once per frame, e.g.
                //    32ms at 30 FPS. This causes short rumble entries to be skipped sometimes.
                // 2. The haptic actuators in gamepads often have slow rise and fall times and are
                //    not able to react quickly enough to changes in requested motor speeds.
                result.durationsMs[rumbleIndex] = patternDurationMs * 2;

                result.lowFrequencyMotorSpeeds[rumbleIndex] = pattern.amplitude[rumbleIndex];
                result.highFrequencyMotorSpeeds[rumbleIndex] = pattern.amplitude[rumbleIndex];
                result.totalDurationMs += result.durationsMs[rumbleIndex];
            }
            return result;
        }

        /// <summary>
        /// Plays a set of predefined haptic patterns
        /// </summary>
        ///
        /// These predefined haptic patterns are played and represented in different ways for iOS,
        /// Android and gamepads.
        ///
        /// On iOS, this function triggers system haptics that are native to iOS.
        /// On Android and gamepads, this function plays a haptic preset represented with a
        /// <c>Pattern</c> that has a similar experience to a system haptics of iOS.
        ///
        /// This is a "fire-and-forget" method. Other functionalities like seeking, looping, and
        /// runtime modulation,  won't work after calling this method.
        /// <param name="presetType">Type of preset represented by a \ref PresetType enum</param>
        public static void PlayPreset(PresetType presetType)
        {
            if (!HapticController.hapticsEnabled)
            {
                return;
            }

            Pattern pattern = PatternForPreset(presetType);
            if (GamepadRumbler.IsConnected())
            {
                GamepadRumble rumble = PatternToRumble(pattern);
                GamepadRumbler.Load(rumble);
                GamepadRumbler.Play();
                return;
            }
#if (UNITY_ANDROID && !UNITY_EDITOR)
            PlayPattern(pattern);
#elif (UNITY_IOS && !UNITY_EDITOR)
            if(DeviceCapabilities.isVersionSupported) {
                LofeltHaptics.TriggerPresetHaptics((int)presetType);
            }
#endif
        }

        static float GetPatternDuration(Pattern pattern)
        {
            if (pattern.time.Length > 0)
            {
                return pattern.time[pattern.time.Length - 1];
            }
            else
            {
                return 0f;
            }
        }

        /// <summary>
        /// Returns the haptic preset duration
        /// </summary>
        ///
        /// The haptic preset duration is based on the Pattern associated with a Preset.
        /// Even though Pattern is only used to play presets for Android, we assume on iOS they have
        /// a similar duration since we don't have any other way of knowing it.
        ///
        /// <param name="presetType"> Type of preset represented by a \ref PresetType enum </param>
        /// <returns>Returns a float with a the preset duration; if the selected preset is `None`, it returns 0</returns>
        public static float GetPresetDuration(PresetType presetType)
        {
            Pattern pattern = PatternForPreset(presetType);
            return GetPatternDuration(pattern);
        }
    }

}
