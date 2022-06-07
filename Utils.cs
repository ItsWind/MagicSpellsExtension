using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.Core.ItemObject;

namespace PaladinMagic
{
    public static class Utils
    {
        public static bool CheckFormationsOnSameSide(Formation f1, Formation f2)
        {
            bool nullCheck = f1 != null && f2 != null && f1.Team != null && f2.Team != null;
            if (!nullCheck) return false;

            return f1.Team.Side.Equals(f2.Team.Side);
        }

        public static Agent? GetAgentClosestToLocation(Vec3 location)
        {
            Agent? agent = null;
            foreach (Agent a in Mission.Current.GetAgentsInRange(location.AsVec2, 0.1f))
            {
                agent = a;
                break;
            }
            return agent;
        }

        private static Dictionary<Agent, float> agentsToCancelEffectedCheer = new();
        public static void DoAgentEffectedCheer(Agent agent)
        {
            agentsToCancelEffectedCheer[agent] = 0.0f;
            agent.HandleCheer(0);
        }
        public static void CheckCancelAgentCheers(float dt)
        {
            foreach (KeyValuePair<Agent, float> kvp in agentsToCancelEffectedCheer.ToList())
            {
                if (kvp.Value + dt >= 1.25f)
                {
                    kvp.Key.CancelCheering();
                    agentsToCancelEffectedCheer.Remove(kvp.Key);
                }
                else
                {
                    agentsToCancelEffectedCheer[kvp.Key] = kvp.Value + dt;
                }
            }
        }

        public static void ModAgentHealth(Agent agent, float mod)
        {
            if (agent.Health + mod > agent.HealthLimit)
                agent.Health = agent.HealthLimit;
            else
                agent.Health += mod;
        }

        public static bool IsMissionWeaponSpell(MissionWeapon weapon)
        {
            if (weapon.Item.ItemType.Equals(ItemTypeEnum.Thrown) && weapon.Item.Name.ToString().Contains("Spell "))
                return true;
            return false;
        }

        public static bool DoesMissionWeaponHeal(MissionWeapon weapon)
        {
            if (weapon.Item.ItemType.Equals(ItemTypeEnum.Thrown) && weapon.Item.Name.ToString().Contains("Healing"))
                return true;
            return false;
        }

        public static void PrintToMessages(string str, int r, int g, int b)
        {
            float[] newValues = { (float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f };
            Color col = new(newValues[0], newValues[1], newValues[2]);
            InformationManager.DisplayMessage(new InformationMessage(str, col));
        }
        public static void PrintToMessages(string str)
        {
            InformationManager.DisplayMessage(new InformationMessage(str));
        }
    }
}
