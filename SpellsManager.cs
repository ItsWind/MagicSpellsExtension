using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public static class SpellsManager
    {
        private static List<SpellData> activeSpells = new();

        private static Dictionary<string, Action<Agent>> spellTypes = new Dictionary<string, Action<Agent>>
        {
            { "BaseHeal",
                (affectedAgent) => 
                {
                    Utils.ModAgentHealth(affectedAgent, 10.0f);
                }
            },
            { "BaseFear",
                (affectedAgent) =>
                {
                    affectedAgent.ChangeMorale(-25.0f);
                }
            },
            { "BaseSlow",
                (affectedAgent) =>
                {
                    affectedAgent.SetMaximumSpeedLimit(1.0f, false);
                }
            }
        };

        public static Dictionary<string, Action<Agent>> SpellFunctions = new Dictionary<string, Action<Agent>>
        {
            { "Spell Healing Bolt",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseHeal", 0, 0, "psys_campfire");
                }
            },
            { "Spell Mass Healing",
                (affectedAgent) =>
                {
                    foreach (Agent a in Mission.Current.GetNearbyAllyAgents(affectedAgent.Position.AsVec2, 10.0f, affectedAgent.Team))
                    {
                        SpellsManager.AddActiveSpell(a, "BaseHeal", 0, 0, "psys_campfire");
                    }
                }
            },
            { "Spell Healing Aura",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseHeal", 15.0f, 2.0f, "psys_campfire");
                }
            },
            { "Spell Fear",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseFear", 0, 0, "psys_campfire");
                }
            },
            { "Spell Slow",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseSlow", 5.0f, 0.1f, "psys_campfire");
                }
            }
        };

        public static void ClearAllActiveSpells()
        {
            activeSpells.Clear();
        }

        public static void AddActiveSpell(Agent affectedAgent, string spellType, float until, float effectEvery = 1.0f, string fxName = "")
        {
            activeSpells.Add(new SpellData(affectedAgent, spellTypes[spellType], until, effectEvery, fxName));
        }

        public static void DoActiveSpellEffects(float dt)
        {
            foreach (SpellData spell in activeSpells.ToList())
                if (spell.Tick(dt))
                    activeSpells.Remove(spell);
        }
    }
}
