
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using UnityEngine;

namespace ExperienceForever
{
    public class LevelBalancePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(SkillClass).GetMethod(nameof(SkillClass.method_2));
        }

        [PatchPrefix]
        internal static bool PatchPrefix(ref float __result, float input, SkillClass __instance, float ___float_2, float ___float_1)
        {
            if (Plugin.config_enabled.Value)
            {
                ___float_2 = 1f;  // Set effectiveness to full (1.0), ignoring any fatigue or other modifiers
                float totalPointsEarned = 0f;
                int iterations = Mathf.CeilToInt(input);
                for (int i = 0; i < iterations; i++)
                {
                    float amountToProcess = Mathf.Min(1f, input);
                    input -= amountToProcess;
                    totalPointsEarned += amountToProcess;  // Since float_2 is 1, num4 and num5 are equivalent from original method
                    ___float_1 += amountToProcess;  // Update points earned without any adjustments
                }

                if (__instance.Id == ESkillId.Metabolism)
                {
                    __result = totalPointsEarned * Plugin.config_metabolism.Value; // use metabolism multiplier
                    Logger.LogInfo("Metabolism skill detected, total points earned: " + totalPointsEarned + " with multiplier: " + Plugin.config_metabolism.Value);
                    return false;
                }

                __result = totalPointsEarned * Plugin.config_skillBonus.Value; //use skillpoint multiplier if not metabolism skill
                Logger.LogInfo("Skill Total points earned: " + totalPointsEarned + " with multiplier: " + Plugin.config_skillBonus.Value);
                return false;
            }

            return true;  // Continue with the original method
        }
    }
}