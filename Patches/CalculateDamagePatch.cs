using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace MagicSpells.Patches
{
    [HarmonyPatch(typeof(SandBox.GameComponents.SandboxAgentApplyDamageModel), nameof(SandBox.GameComponents.SandboxAgentApplyDamageModel.CalculateDamage))]
    internal class CalculateDamagePatch
    {
        [HarmonyPostfix]
        private static void Postfix(ref float __result, ref AttackInformation attackInformation, MissionWeapon weapon)
        {
            if (Utils.IsMissionWeaponSpell(weapon) && Utils.DoesMissionWeaponHeal(weapon))
            {
                bool agentsOnSameSide = attackInformation.AttackerFormation.Team.Side.Equals(attackInformation.VictimFormation.Team.Side);
                if (agentsOnSameSide)
                    __result = 0.0f;
            }
        }
    }
}
