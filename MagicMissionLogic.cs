using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class MagicMissionLogic : MissionLogic
    {
        public override void OnMissionTick(float dt)
        {
            SubModule.AgentsTick(dt);
        }

        protected override void OnEndMission()
        {
            SubModule.ActiveAgentEffects.Clear();
            SavedVarsManager.ClearAllVars();
        }
    }
}
