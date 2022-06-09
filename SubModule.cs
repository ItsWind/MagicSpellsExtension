using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace PaladinMagic
{
    public class SubModule : MBSubModuleBase
    {
        public static Config Config = new Config();

        protected override void OnSubModuleLoad()
        {
            new Harmony("PaladinMagic").PatchAll();
        }

        protected override void OnApplicationTick(float dt)
        {
            bool isInScene = Game.Current != null && Mission.Current != null && Mission.Current.Scene != null;
            if (isInScene)
            {
                Utils.CheckCancelAgentCheers(dt);
                SpellsManager.DoActiveSpellEffects(dt);
                AgentFXManager.DoTick(dt);
            }
        }

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            SpellsManager.ClearAllActiveSpells();
            AgentFXManager.ClearAllAgentFX();
        }
    }
}