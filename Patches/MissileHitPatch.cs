using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace MagicSpells.Patches
{
    [HarmonyPatch(typeof(Mission), "MissileHitCallback")]
    internal class MissileHitPatch
    {
        [HarmonyPostfix]
        private static void Postfix(Mission __instance, ref AttackCollisionData collisionData, Agent attacker, Agent victim)
        {
            if (victim != null)
            {
                Dictionary<int, Mission.Missile> missiles = Traverse.Create(__instance).Field("_missiles").GetValue() as Dictionary<int, Mission.Missile>;
                MissionWeapon weaponUsed = missiles[collisionData.AffectorWeaponSlotOrMissileIndex].Weapon;
                if (Utils.IsMissionWeaponSpell(weaponUsed))
                {
                    Action<Agent> funcToDo;
                    try
                    {
                        funcToDo = SpellsManager.SpellFunctions[weaponUsed.Item.Name.ToString()];
                    }
                    catch (Exception e)
                    {
                        Utils.PrintToMessages("spell function not found");
                        return;
                    }

                    bool agentsOnSameSide = attacker.Team.Side.Equals(victim.Team.Side);

                    if (Utils.DoesMissionWeaponHeal(weaponUsed))
                    {
                        if (agentsOnSameSide)
                            funcToDo(victim);
                    }
                    else
                    {
                        if (!agentsOnSameSide)
                            funcToDo(victim);
                    }
                }
            }
        }
    }
}
