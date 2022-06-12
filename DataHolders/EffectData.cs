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
        public Agent Attacker;
        public Agent Victim;
        public string FXName;
        public Action<Agent> PerformFunc;
        public float TimeLeft;

        private float repeatEvery;

        public EffectData(Agent attacker, Agent victim, string fxName, Action<Agent> func, float until = 0.0f, float repeat = 0.0f)
        {
            Attacker = attacker;
            Victim = victim;
            FXName = fxName;
            PerformFunc = func;
            TimeLeft = until;
            repeatEvery = repeat;
        }

        private float repeatTimer = 0.0f;
        public void PerformTick(float dt)
        {
            repeatTimer += dt;
            if (repeatTimer >= repeatEvery)
            {
                repeatTimer = 0.0f;
                PerformFunc(Victim);
            }

            TimeLeft -= dt;
            if (TimeLeft <= 0.0f)
            {
                SubModule.RemoveEffectFromAgent(Victim, this);
            }
        }
    }
}
