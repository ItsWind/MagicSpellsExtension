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
        public static Dictionary<Agent, AgentFXData> ActiveFXData = new Dictionary<Agent, AgentFXData>();

        public static AgentFXData? GetAgentFXData(Agent agent)
        {
            AgentFXData data;
            try
            {
                data = ActiveFXData[agent];
            }
            catch (Exception ex)
            {
                return null;
            }
            return data;
        }

        public static void DoTick(float dt)
        {
            foreach (KeyValuePair<Agent, AgentFXData> pair in ActiveFXData.ToList())
            {
                pair.Value.Tick(dt);
            }
        }

        public static void ClearAllAgentFX()
        {
            ActiveFXData.Clear();
        }

        public static void RemoveAgentFX(Agent agent)
        {
            AgentFXData? data = GetAgentFXData(agent);
            if (data != null)
            {
                Utils.PrintToMessages("fx data found trying to remove in manager");
                data.RemoveFX();
                Utils.PrintToMessages(ActiveFXData.Remove(agent).ToString());
            }
        }

        public static void DoAgentFX(Agent agent, string fxName, float until)
        {
            RemoveAgentFX(agent);
            ActiveFXData[agent] = new AgentFXData(agent, fxName, until);
        }
    }
}
