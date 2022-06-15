using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public static class SavedVarsManager
    {
        public static Dictionary<Agent, Dictionary<string, Object>> SavedVars = new();

        public static void AddAgentVar(Agent agent, string varId, Object value)
        {
            try
            {
                if (!SavedVars[agent].ContainsKey(varId))
                    SavedVars[agent].Add(varId, value);
            }
            catch (KeyNotFoundException e)
            {
                SavedVars[agent] = new();
                SavedVars[agent].Add(varId, value);
            }
        }

        public static Object? UseAgentVar(Agent agent, string varId, bool remove = true)
        {
            try
            {
                Object objToReturn = SavedVars[agent][varId];
                if (remove)
                    SavedVars[agent].Remove(varId);
                return objToReturn;
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }

        public static bool AgentHasEffectVar(Agent agent, string effectVarIdBase)
        {
            try
            {
                foreach (KeyValuePair<string, Object> kvp in SavedVars[agent].ToList())
                    if (kvp.Key.Contains(effectVarIdBase))
                        return true;
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }
            return false;
        }
    }
}
