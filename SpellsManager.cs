using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace PaladinMagic
{
    public static class SpellsManager
    {
        private static List<SpellData> activeSpells = new();

        private static Dictionary<string, Action<Agent>> spellTypes = new Dictionary<string, Action<Agent>>
        {
            { "BaseHeal",
                (affectedAgent) => 
                {
                    Utils.ModAgentHealth(affectedAgent, SpellsManager.GetBaseHealAmt());
                }
            },
            { "BaseFear",
                (affectedAgent) =>
                {
                    affectedAgent.ChangeMorale(-25.0f);
                }
            }
        };

        public static Dictionary<string, Action<Agent>> SpellFunctions = new Dictionary<string, Action<Agent>>
        {
            { "Spell Healing Bolt",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseHeal", 0, 0, true, "psys_campfire");
                }
            },
            { "Spell Mass Healing",
                (affectedAgent) =>
                {
                    foreach (Agent a in Mission.Current.GetNearbyAllyAgents(affectedAgent.Position.AsVec2, 10.0f, affectedAgent.Team))
                    {
                        SpellsManager.AddActiveSpell(a, "BaseHeal", 0, 0, true, "psys_campfire");
                    }
                }
            },
            { "Spell Healing Aura",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseHeal", 15.0f, 2.0f, true, "psys_campfire");
                }
            },
            { "Spell Fear",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseFear", 0, 0, false, "psys_campfire");
                }
            }
        };

        public static float GetBaseHealAmt()
        {
            return (float)SubModule.Config.GetKeyValue("baseHealAmount");
        }

        public static void ClearAllActiveSpells()
        {
            activeSpells.Clear();
        }

        public static void AddActiveSpell(Agent affectedAgent, string spellType, float until, float effectEvery = 1.0f, bool positiveEffect = true, string fxName = "")
        {
            activeSpells.Add(new SpellData(affectedAgent, spellTypes[spellType], until, effectEvery, positiveEffect, fxName));
        }

        public static void DoActiveSpellEffects(float dt)
        {
            foreach (SpellData spell in activeSpells.ToList())
                if (spell.Tick(dt))
                    activeSpells.Remove(spell);
        }
    }
}
