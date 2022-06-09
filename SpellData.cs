using System;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class SpellData
    {
        public Agent AffectedAgent;
        public Action<Agent> RepeatingEffect;
        public float RepeatFor;
        public float EffectEvery;

        private float effectTimer;
        private float timer;

        public SpellData(Agent agent, Action<Agent> func, float repeatTime, float effectEvery = 1.0f, string fxName = "")
        {
            this.AffectedAgent = agent;
            this.RepeatingEffect = func;
            this.RepeatFor = repeatTime;
            this.EffectEvery = effectEvery;

            if (!fxName.Equals(""))
                AgentFXManager.DoAgentFX(this.AffectedAgent, fxName, this.RepeatFor < 1.5f ? 1.5f : this.RepeatFor);

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
