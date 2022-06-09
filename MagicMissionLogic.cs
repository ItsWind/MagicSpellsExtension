using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class MagicMissionLogic : MissionLogic
    {
        public override void OnMissionTick(float dt)
        {
            SpellsManager.DoActiveSpellEffects(dt);
            AgentFXManager.DoTick(dt);
        }
    }
}
