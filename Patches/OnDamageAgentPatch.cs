using HarmonyLib;
using System;
using TaleWorlds.MountAndBlade;

namespace PaladinMagic.Patches
{
    [HarmonyPatch(typeof(SandBox.GameComponents.SandboxAgentApplyDamageModel), nameof(SandBox.GameComponents.SandboxAgentApplyDamageModel.CalculateDamage))]
    internal class OnDamageAgentPatch
    {
        private static AttackInformation attackInfo;
        private static MissionWeapon weaponUsed;

        [HarmonyPrefix]
        public static void Prefix(ref AttackInformation attackInformation, ref AttackCollisionData collisionData, in MissionWeapon weapon)
        {
            attackInfo = attackInformation;
            weaponUsed = weapon;
        }

        [HarmonyPostfix]
        public static float Postfix(float original)
        {
            if (Utils.IsMissionWeaponSpell(weaponUsed))
            {
                Agent? affectedAgent = Utils.GetAgentClosestToLocation(attackInfo.VictimAgentPosition);
                Action<Agent> funcToDo;
                try
                {
                    funcToDo = SpellsManager.SpellFunctions[weaponUsed.Item.Name.ToString()];
                }
                catch (Exception e)
                {
                    return original;
                }

                bool agentsOnSameSide = Utils.CheckFormationsOnSameSide(attackInfo.AttackerFormation, attackInfo.VictimFormation);

                if (Utils.DoesMissionWeaponHeal(weaponUsed))
                {
                    if (agentsOnSameSide)
                        SpellsManager.SpellFunctions[weaponUsed.Item.Name.ToString()](affectedAgent);
                    return 0.0f;
                }
                else
                {
                    if (!agentsOnSameSide)
                        SpellsManager.SpellFunctions[weaponUsed.Item.Name.ToString()](affectedAgent);
                    return original;
                }
            }
            return original;
        }
    }
}
