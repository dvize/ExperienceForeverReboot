
using BepInEx;
using BepInEx.Configuration;

namespace ExperienceForever
{
    [BepInPlugin("org.rlenworld.plugins.experienceforever", "Experience Forever", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigEntry<bool> config_enabled;
        public static ConfigEntry<float> config_skillBonus;
        public static ConfigEntry<float> config_metabolism;

        public static Plugin Instance
        {
            get; private set;
        }

        private void Awake()
        {
            config_enabled = Config.Bind(
                "General",
                "Enabled",
                true,
                "Enables/disables Experience Forever."
            );

            config_skillBonus = Config.Bind(
                "General",
                "Skill Point Multiplier",
                1.0f,
                "Sets your levelling speed multiplier to this value. (1 is normal)"
            );

            config_metabolism = Config.Bind(
                "Specifics",
                "Metabolism Multiplier",
                0.1f,
                "XP rewards for Metabolism are very high, so this setting will allow you to adjust earnings to be more in line with other skills."
            );

            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Instance = this;
            new LevelBalancePatch().Enable();
        }

    }
}
