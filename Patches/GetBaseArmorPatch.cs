using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace MagicSpells.Patches
{
    [HarmonyPatch(typeof(Agent), nameof(Agent.GetBaseArmorEffectivenessForBodyPart))]
    internal class GetBaseArmorPatch
    {
        [HarmonyPostfix]
        private static void Postfix(ref float __result, Agent __instance)
        {
            if (SavedVarsManager.AgentHasEffectVar(__instance, "checkEffectBreakArmor"))
                __result *= 0.1f;
            else if (SavedVarsManager.AgentHasEffectVar(__instance, "checkEffectStoneflesh"))
                __result = __result < 10.0f ? 20.0f : __result * 2.0f;
        }
    }
}