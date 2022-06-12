using HarmonyLib;
using MagicSpells.DataHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MagicSpells.Patches
{
    [HarmonyPatch(typeof(Mission), "MissileHitCallback")]
    internal class MissileHitPatch
    {
        private static void addEffectWithNoEnemyEffect(Agent attacker, Agent victim, MissionWeapon weapon, EffectData data)
        {
            if (Utils.DoesMissionWeaponHeal(weapon) == attacker.Team.Side.Equals(victim.Team.Side))
                SubModule.AddEffectToAgent(victim, data);
        }

        [HarmonyPostfix]
        private static void Postfix(Mission __instance, ref AttackCollisionData collisionData, Agent attacker, Agent victim, Vec3 missilePosition)
        {
            Dictionary<int, Mission.Missile> missiles = Traverse.Create(__instance).Field("_missiles").GetValue() as Dictionary<int, Mission.Missile>;
            MissionWeapon weaponUsed = missiles[collisionData.AffectorWeaponSlotOrMissileIndex].Weapon;
            if (Utils.IsMissionWeaponSpell(weaponUsed))
            {
                EffectData? spellEffectData;
                try
                {
                    spellEffectData = Utils.GetSpellEffectData(victim, weaponUsed.Item.Name.ToString());
                }
                catch (KeyNotFoundException e)
                {
                    Utils.PrintToMessages("Spell function not found");
                    return;
                }
                
                if (spellEffectData.AOERadius == 0.0f && victim != null)
                {
                    addEffectWithNoEnemyEffect(attacker, victim, weaponUsed, spellEffectData);
                }
                else if (missilePosition != null && spellEffectData.AOERadius > 0.0f)
                {
                    foreach (Agent agent in Mission.Current.GetNearbyAgents(missilePosition.AsVec2, spellEffectData.AOERadius))
                        addEffectWithNoEnemyEffect(attacker, agent, weaponUsed, spellEffectData);
                }
            }
        }
    }
}
