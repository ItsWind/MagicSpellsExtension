using HarmonyLib;
using MagicSpells.DataHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class SubModule : MBSubModuleBase
    {
        //public static Config Config = new Config();
        //public static Dictionary<Agent, AgentEffectData> ActiveAgentData = new();
        public static Dictionary<Agent, List<EffectData>> ActiveAgentEffects = new();

        protected override void OnSubModuleLoad()
        {
            new Harmony("MagicSpells").PatchAll();
        }
        
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            SubModule.ActiveAgentEffects.Clear();

            mission.AddMissionBehavior(new MagicMissionLogic());
        }

        public static void AgentsTick(float dt)
        {
            foreach (KeyValuePair<Agent, List<EffectData>> kvp in ActiveAgentEffects.ToList())
                foreach (EffectData effect in kvp.Value.ToList())
                    effect.PerformTick(dt);
        }

        public static void AddEffectToAgentsNear(Agent attacker, Vec3 position, string spellName, bool needsSameSide, float radius = 5.0f)
        {
            foreach (Agent agent in Mission.Current.AllAgents.ToList())
                if (agent.Position.Distance(position) <= radius)
                    SubModule.AddEffectToAgent(attacker, agent, spellName, needsSameSide);
        }

        public static void AddEffectToAgent(Agent attacker, Agent victim, string spellName, bool needsSameSide)
        {
            EffectData? effect = SubModule.GetSpellEffectData(attacker, victim, spellName);
            if (effect == null)
                return;

            if (victim.IsHuman)
            {
                if (needsSameSide == attacker.Team.Side.Equals(victim.Team.Side))
                {
                    try
                    {
                        ActiveAgentEffects[victim].Add(effect);
                    }
                    catch (KeyNotFoundException e)
                    {
                        ActiveAgentEffects[victim] = new();
                        ActiveAgentEffects[victim].Add(effect);
                    }
                }
            }
        }

        public static void RemoveEffectFromAgent(Agent agent, EffectData effect)
        {
            try
            {
                ActiveAgentEffects[agent].Remove(effect);
            }
            catch (Exception e)
            {
                Utils.PrintToMessages("Oopsie found");
            }
        }

        public static EffectData? GetSpellEffectData(Agent attacker, Agent victim, string spellName)
        {
            switch (spellName)
            {
                case "Spell Healing Bolt":
                    return new EffectData(attacker, victim, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("healing bolt healing");
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    });
                case "Spell Healing Aura":
                    return new EffectData(attacker, victim, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("healing aura healing");
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    }, 15.0f, 2.0f);
                case "Spell Mass Healing":
                    return new EffectData(attacker, victim, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("mass healing");
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    }, 0, 0);
                case "Spell Fear":
                    return new EffectData(attacker, victim, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("fear");
                        affectedAgent.SetMorale(affectedAgent.GetMorale() - 10.0f);
                    }, 0, 0);
                case "Spell Slow":
                    return new EffectData(attacker, victim, "psys_campfire", (affectedAgent) =>
                    {
                        Utils.PrintToMessages("slow");
                        affectedAgent.SetMaximumSpeedLimit(1.0f, false);
                    }, 5.0f, 0.1f);
                default:
                    return null;
            }
        }
    }
}