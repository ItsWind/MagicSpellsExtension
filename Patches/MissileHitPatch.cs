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
        enum SpellType
        {
            Direct,
            AOE
        }

        private static Dictionary<string, SpellType> spellTypes = new Dictionary<string, SpellType>
        {
            { "Spell Mass Healing",
                SpellType.AOE
            },
            { "Spell Fear",
                SpellType.AOE
            },
            { "Spell Slow",
                SpellType.AOE
            },
            { "Spell Healing Bolt",
                SpellType.Direct
            },
            { "Spell Healing Aura",
                SpellType.Direct
            }
        };

        [HarmonyPostfix]
        private static void Postfix(Mission __instance, ref AttackCollisionData collisionData, Agent attacker, Agent victim, Vec3 missilePosition)
        {
            Dictionary<int, Mission.Missile> missiles = Traverse.Create(__instance).Field("_missiles").GetValue() as Dictionary<int, Mission.Missile>;
            if (missiles == null)
                return;

            MissionWeapon weaponUsed = missiles[collisionData.AffectorWeaponSlotOrMissileIndex].Weapon;
            if (Utils.IsMissionWeaponSpell(weaponUsed))
            {
                string weaponUsedName = weaponUsed.Item.Name.ToString();
                SpellType type = SpellType.Direct;

                try
                {
                    type = spellTypes[weaponUsedName];
                }
                catch (KeyNotFoundException) { }

                switch (type)
                {
                    case SpellType.Direct:
                        {
                            if (victim == null)
                                return;

                            SubModule.AddEffectToAgent(attacker, victim, weaponUsedName, Utils.DoesMissionWeaponHeal(weaponUsed));
                            break;
                        }
                    case SpellType.AOE:
                        {
                            if (missilePosition == null)
                                return;

                            SubModule.AddEffectToAgentsNear(attacker, missilePosition, weaponUsedName, Utils.DoesMissionWeaponHeal(weaponUsed));
                            break;
                        }
                }
            }
        }
    }
}
