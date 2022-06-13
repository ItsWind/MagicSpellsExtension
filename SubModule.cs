using HarmonyLib;
using MagicSpells.DataHolders;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class SubModule : MBSubModuleBase
    {
        //public static Config Config = new Config();
        public static Dictionary<Agent, List<EffectData>> ActiveAgentEffects = new();

        protected override void OnSubModuleLoad()
        {
            new Harmony("MagicSpells").PatchAll();
        }
        
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            mission.AddMissionBehavior(new MagicMissionLogic());
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
                bool agentsOnSameSide = attacker.Team.Side.Equals(victim.Team.Side);
                bool canAddEffect = needsSameSide == agentsOnSameSide;
                if (canAddEffect)
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
            /* psys_campfire = nice tiny fire
             * psys_blaze_1, prt_torch_flame = tiny flame with a lot of smoke
             * psys_sea_foam_a = kind of web like
             * sea_side_water_splash = forceful impact into ground / USE FOR SLOW
             * main_menu_fast_smoke = Small amount of smoke
             * torch_burning_fire_smoke = medium amount of smoke
             * psys_bug_fly_1 = yellowish tiny particles / USE FOR HEALING
             * 
             * */
            switch (spellName)
            {
                case "Spell Healing Bolt":
                    return new EffectData(attacker, victim, "psys_bug_fly_1", (affectedAgent) =>
                    {
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    }, "magicspells/effect/heal", true);
                case "Spell Healing Aura":
                    return new EffectData(attacker, victim, "psys_bug_fly_1", (affectedAgent) =>
                    {
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    }, "magicspells/effect/heal", true, 15.0f, 2.0f);
                case "Spell Mass Healing":
                    return new EffectData(attacker, victim, "psys_bug_fly_1", (affectedAgent) =>
                    {
                        Utils.ModAgentHealth(affectedAgent, 10.0f);
                    }, "magicspells/effect/heal", true);
                case "Spell Fear":
                    return new EffectData(attacker, victim, "main_menu_fast_smoke", (affectedAgent) =>
                    {
                        affectedAgent.SetMorale(affectedAgent.GetMorale() - 10.0f);
                    }, "magicspells/effect/heal", true);
                case "Spell Slow":
                    return new EffectData(attacker, victim, "sea_side_water_splash", (affectedAgent) =>
                    {
                        SavedVarsManager.AddAgentVar(affectedAgent, "originalMaxMoveSpeed", affectedAgent.GetMaximumSpeedLimit());
                        affectedAgent.SetMaximumSpeedLimit(1.0f, false);
                    }, "magicspells/effect/heal", true, 5.0f, 0.1f, (affectedAgent) =>
                    {
                        float? setTo = (float)SavedVarsManager.UseAgentVar(affectedAgent, "originalMaxMoveSpeed");
                        if (setTo != null)
                            affectedAgent.SetMaximumSpeedLimit((float)setTo, false);
                    });
                default:
                    return null;
            }
        }
    }
}