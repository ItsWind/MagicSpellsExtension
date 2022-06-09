using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class SubModule : MBSubModuleBase
    {
        //public static Config Config = new Config();

        protected override void OnSubModuleLoad()
        {
            new Harmony("MagicSpells").PatchAll();
        }

        /*protected override void OnApplicationTick(float dt)
        {
            bool isInScene = Game.Current != null && Mission.Current != null && Mission.Current.Scene != null;
            if (isInScene)
            {
                SpellsManager.DoActiveSpellEffects(dt);
                AgentFXManager.DoTick(dt);
            }
        }*/

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            mission.AddMissionBehavior(new MagicMissionLogic());
        }

        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            SpellsManager.ClearAllActiveSpells();
            AgentFXManager.ClearAllAgentFX();
        }

        public override void OnGameLoaded(Game game, object initializerObject)
        {
            SpellsManager.ClearAllActiveSpells();
            AgentFXManager.ClearAllAgentFX();
        }
    }
}