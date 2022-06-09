using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public static class AgentFXManager
    {
        public static Dictionary<Agent, List<AgentFXData>> ActiveFXData = new Dictionary<Agent, List<AgentFXData>>();

        public static void DoTick(float dt)
        {
            foreach (KeyValuePair<Agent, List<AgentFXData>> pair in ActiveFXData.ToList())
            {
                foreach (AgentFXData data in pair.Value.ToList())
                {
                    data.Tick(dt);
                }
            }
        }

        public static void ClearAllAgentFX()
        {
            ActiveFXData.Clear();
        }

        public static void RemoveAgentFX(Agent agent)
        {
            if (ActiveFXData.ContainsKey(agent))
            {
                foreach (AgentFXData data in ActiveFXData[agent].ToList())
                {
                    data.RemoveFX();
                }
            }
        }

        public static void DoAgentFX(Agent agent, string fxName, float until)
        {
            try
            {
                ActiveFXData[agent].Add(new AgentFXData(agent, fxName, until));
            }
            catch (KeyNotFoundException e)
            {
                ActiveFXData[agent] = new List<AgentFXData>();
                ActiveFXData[agent].Add(new AgentFXData(agent, fxName, until));
            }
        }
    }
}
