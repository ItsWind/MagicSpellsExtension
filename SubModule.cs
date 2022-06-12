using HarmonyLib;
using MagicSpells.DataHolders;
using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class SubModule : MBSubModuleBase
    {
        //public static Config Config = new Config();
        public static Dictionary<Agent, AgentEffectData> ActiveAgentData = new();

        protected override void OnSubModuleLoad()
        {
            new Harmony("MagicSpells").PatchAll();
        }
        
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            SubModule.ActiveAgentData.Clear();

            mission.AddMissionBehavior(new MagicMissionLogic());
        }

        public static void AgentsTick(float dt)
        {
            foreach(AgentEffectData agentData in ActiveAgentData.Values)
                agentData.DoTick(dt);
        }

        public static void AddEffectToAgent(List<Agent> agents, EffectData effect)
        {
            foreach (Agent agent in agents)
                SubModule.AddEffectToAgent(agent, effect);
        }
        public static void AddEffectToAgent(Agent agent, EffectData effect)
        {
            try
            {
                ActiveAgentData[agent].AddEffect(effect);
            }
            catch(KeyNotFoundException e)
            {
                ActiveAgentData[agent] = new AgentEffectData(agent);
                ActiveAgentData[agent].AddEffect(effect);
            }
        }
    }
}