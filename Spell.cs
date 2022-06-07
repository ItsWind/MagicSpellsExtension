using System;
using TaleWorlds.MountAndBlade;

namespace PaladinMagic
{
    public class Spell
    {
        public Agent AffectedAgent;
        public Action<Agent> RepeatingEffect;
        public float RepeatFor;
        public float EffectEvery;

        private float effectTimer;
        private float timer;

        public Spell(Agent agent, Action<Agent> func, float repeatTime, float effectEvery = 1.0f, bool positiveEffect = true)
        {
            this.AffectedAgent = agent;
            this.RepeatingEffect = func;
            this.RepeatFor = repeatTime;
            this.EffectEvery = effectEvery;

            if (positiveEffect)
                Utils.DoAgentEffectedCheer(this.AffectedAgent);
            this.doEffect();
        }

        private void doEffect()
        {
            if (this.AffectedAgent != null && this.AffectedAgent.IsActive())
                RepeatingEffect(this.AffectedAgent);
        }

        public bool Tick(float dt)
        {
            this.timer += dt;
            this.effectTimer += dt;

            if (this.effectTimer >= this.EffectEvery)
            {
                doEffect();
                this.effectTimer = 0.0f;
            }

            if (this.timer >= this.RepeatFor)
            {
                return true;
            }
            return false;
        }
    }
}
