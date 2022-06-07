using HarmonyLib;
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
            if (Utils.DoesMissionWeaponHeal(weaponUsed))
            {
                Utils.PrintToMessages("HEALING SPELL USED");
                Agent? affectedAgent = Utils.GetAgentClosestToLocation(attackInfo.VictimAgentPosition);
                if (affectedAgent == null)
                    return 0.0f;

                bool nullCheck = attackInfo.AttackerFormation != null && attackInfo.VictimFormation != null && attackInfo.AttackerFormation.Team != null && attackInfo.VictimFormation.Team != null;
                if (nullCheck && attackInfo.AttackerFormation.Team.Side.Equals(attackInfo.VictimFormation.Team.Side))
                    SpellsManager.SpellFunctions[weaponUsed.Item.Name.ToString()](affectedAgent);

                return 0.0f;
            }
            return original;
        }
    }
}
