using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;

namespace PaladinMagic
{
    public static class SpellsManager
    {
        private static List<Spell> activeSpells = new();

        private static Dictionary<string, Action<Agent>> spellTypes = new Dictionary<string, Action<Agent>>
        {
            { "BaseHeal",
                (affectedAgent) => {
                    Utils.ModAgentHealth(affectedAgent, SpellsManager.GetBaseHealAmt());
                }
            }
        };

        public static Dictionary<string, Action<Agent>> SpellFunctions = new Dictionary<string, Action<Agent>>
        {
            { "Spell Healing Bolt",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseHeal", 0, 0);
                }
            },
            { "Spell Mass Healing",
                (affectedAgent) =>
                {
                    foreach (Agent a in Mission.Current.GetNearbyAllyAgents(affectedAgent.Position.AsVec2, 10.0f, affectedAgent.Team))
                    {
                        SpellsManager.AddActiveSpell(a, "BaseHeal", 0, 0);
                    }
                }
            },
            { "Spell Healing Aura",
                (affectedAgent) =>
                {
                    SpellsManager.AddActiveSpell(affectedAgent, "BaseHeal", 15.0f, 2.0f);
                }
            }
        };

        public static float GetBaseHealAmt()
        {
            return (float)SubModule.Config.GetKeyValue("baseHealAmount");
        }

        public static void ClearActiveSpells()
        {
            activeSpells.Clear();
        }

        public static void AddActiveSpell(Agent affectedAgent, string spellType, float until, float effectEvery = 1.0f, bool positiveEffect = true)
        {
            activeSpells.Add(new Spell(affectedAgent, spellTypes[spellType], until, effectEvery, positiveEffect));
        }

        public static void DoActiveSpellEffects(float dt)
        {
            foreach (Spell spell in activeSpells.ToList())
                if (spell.Tick(dt))
                    activeSpells.Remove(spell);
        }
    }
}
