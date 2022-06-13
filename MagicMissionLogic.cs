using MagicSpells.DataHolders;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.MountAndBlade;
using TaleWorlds.InputSystem;
using TaleWorlds.Engine;
using TaleWorlds.Library;

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
            if (Input.IsKeyPressed(InputKey.Apostrophe))
            {
                int evtIdFromString = SoundEvent.GetEventIdFromString("magicspells/effect/heal");
                Utils.PrintToMessages(evtIdFromString.ToString());
                SoundEvent evt = SoundEvent.CreateEvent(evtIdFromString, Mission.Current.Scene);
                Utils.PrintToMessages(evt.PlayInPosition(Agent.Main.Position + new Vec3(0, 0, 1f)).ToString());
            }
        }

        protected override void OnEndMission()
        {
            SubModule.ActiveAgentEffects.Clear();
            SavedVarsManager.SavedVars.Clear();
        }
    }
}
