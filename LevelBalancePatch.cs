
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using UnityEngine;

namespace ExperienceForeverReboot
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
            if (!Plugin.enabledPlugin.Value)
            {
                return true;
            }

            if (__instance.Id == ESkillId.Metabolism && Plugin.useOriginalMetabolism.Value)
            {
                // Use original metabolism skill
                return true;
            }

            ___float_2 = 1f;  // Effectiveness is always full
            float totalPointsEarned = 0f;

            int iterations = Mathf.CeilToInt(input);
            for (int i = 0; i < iterations; i++)
            {
                float amountToProcess = Mathf.Min(1f, input);
                input -= amountToProcess;
                totalPointsEarned += amountToProcess;  // Since float_2 is 1, num4 and num5 are equivalent from original method
                ___float_1 += amountToProcess;
            }

            float multiplier = (__instance.Id == ESkillId.Metabolism) ? Plugin.metabolismMultiplier.Value : Plugin.skillMultiplier.Value;
            __result = totalPointsEarned * multiplier;

#if DEBUG
            string skillType = (__instance.Id == ESkillId.Metabolism) ? "Metabolism" : __instance.Id.ToString();
            Logger.LogInfo($"{skillType} skill detected, total points earned: {totalPointsEarned} with multiplier: {multiplier}");
#endif

            return false; // Skip the original method
        }
    }
}