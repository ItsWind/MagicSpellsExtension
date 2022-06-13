using MagicSpells.DataHolders;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class MagicMissionLogic : MissionLogic
    {
        private void agentsTick(float dt)
        {
            foreach (KeyValuePair<Agent, List<EffectData>> kvp in SubModule.ActiveAgentEffects.ToList())
                foreach (EffectData effect in kvp.Value.ToList())
                    effect.PerformTick(dt);
        }

        public override void OnMissionTick(float dt)
        {
            this.agentsTick(dt);
        }

        protected override void OnEndMission()
        {
            SubModule.ActiveAgentEffects.Clear();
            SavedVarsManager.SavedVars.Clear();
        }
    }
}
