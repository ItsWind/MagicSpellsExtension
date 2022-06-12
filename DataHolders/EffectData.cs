using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace MagicSpells.DataHolders
{
    public class EffectData
    {
        public Agent AffectedAgent;
        public string FXName;
        public Action<Agent> PerformFunc;
        public float TimeLeft;
        public float AOERadius;

        private float repeatEvery;

        public EffectData(Agent agent, string fxName, Action<Agent> func, float until, float repeat = 1.0f, float aoeRadius = 0.0f)
        {
            AffectedAgent = agent;
            FXName = fxName;
            PerformFunc = func;
            TimeLeft = until;
            repeatEvery = repeat;
            AOERadius = aoeRadius;
        }

        private float repeatTimer = 0.0f;
        public void PerformTick(float dt)
        {
            repeatTimer += dt;
            if (repeatTimer >= repeatEvery)
            {
                repeatTimer = 0.0f;
                PerformFunc(AffectedAgent);
            }

            TimeLeft -= dt;
            if (TimeLeft <= 0.0f)
            {
                SubModule.ActiveAgentData[AffectedAgent].RemoveEffect(this);
            }
        }
    }
}
