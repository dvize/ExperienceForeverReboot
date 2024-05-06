
using BepInEx;
using BepInEx.Configuration;

namespace ExperienceForeverReboot
{
    [BepInPlugin("com.dvize.ExperienceForeverReboot", "ExperienceForeverReboot", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> enabledPlugin;
        public static ConfigEntry<float> skillMultiplier;
        public static ConfigEntry<float> metabolismMultiplier;
        public static ConfigEntry<bool> useOriginalMetabolism;

        public static Plugin Instance
        {
            get; private set;
        }

        private void Awake()
        {
            enabledPlugin = Config.Bind(
                "General",
                "Enabled",
                true,
                "Enables/disables Experience Forever."
            );

            skillMultiplier = Config.Bind(
                "General",
                "Skill Point Multiplier",
                1.0f,
                "Sets your levelling speed multiplier to this value. (1 is normal)"
            );

            metabolismMultiplier = Config.Bind(
                "Metabolism",
                "Metabolism Multiplier",
                0.1f,
                "XP rewards for Metabolism are very high, so this setting will allow you to adjust earnings to be more in line with other skills."
            );

            useOriginalMetabolism = Config.Bind(
                "Metabolism",
                "Use Original Metabolism",
                true,
                "Enables/disables Experience Forever."
            );

            Instance = this;
            new LevelBalancePatch().Enable();
        }

    }
}
