using HarmonyLib;
using System.Collections.Generic;
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

        enum SpellHarm
        {
            Debuff,
            Buff
        }

        private static Dictionary<string, SpellHarm> spellHarms = new Dictionary<string, SpellHarm>
        {
            { "Spell Stoneflesh",
                SpellHarm.Buff
            },
            { "Spell Healing Bolt",
                SpellHarm.Buff
            },
            { "Spell Healing Aura",
                SpellHarm.Buff
            },
            { "Spell Mass Healing",
                SpellHarm.Buff
            }
        };
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
            { "Spell Fireball",
                SpellType.AOE
            },
            { "Spell Break Armor",
                SpellType.AOE
            },
            { "Spell Stoneflesh",
                SpellType.AOE
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
                SpellHarm harm = SpellHarm.Debuff;
                try
                {
                    type = spellTypes[weaponUsedName];
                }
                catch (KeyNotFoundException) { }
                try
                {
                    harm = spellHarms[weaponUsedName];
                }
                catch (KeyNotFoundException) { }

                switch (type)
                {
                    case SpellType.Direct:
                        {
                            if (victim == null)
                                return;

                            SubModule.AddEffectToAgent(attacker, victim, weaponUsedName, harm == SpellHarm.Buff ? true : false);
                            break;
                        }
                    case SpellType.AOE:
                        {
                            if (missilePosition == null)
                                return;

                            SubModule.AddEffectToAgentsNear(attacker, missilePosition, weaponUsedName, harm == SpellHarm.Buff ? true : false);
                            break;
                        }
                }
            }
        }
    }
}
