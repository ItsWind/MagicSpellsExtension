using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Library;

namespace MagicSpells.DataHolders
{
    public class EffectData
    {
        public Agent Attacker;
        public Agent Victim;
        public string FXName;
        public Action<Agent> PerformFunc;
        public float TimeLeft;

        private GameEntity fxObj = GameEntity.CreateEmpty(Mission.Current.Scene);
        private float repeatEvery;

        public EffectData(Agent attacker, Agent victim, string fxName, Action<Agent> func, float until = 1.5f, float repeat = 0.0f)
        {
            Attacker = attacker;
            Victim = victim;
            FXName = fxName;
            PerformFunc = func;
            TimeLeft = until;
            repeatEvery = repeat;

            if (repeatEvery <= 0.0f)
                PerformFunc(Victim);

            MatrixFrame mf = new MatrixFrame(Mat3.Identity, new Vec3());
            ParticleSystem.CreateParticleSystemAttachedToEntity(fxName, fxObj, ref mf);
        }

        private float repeatTimer = 0.0f;
        public void PerformTick(float dt)
        {
            fxObj.SetLocalPosition(Victim.Position);

            repeatTimer += dt;
            if (repeatEvery > 0.0f && repeatTimer >= repeatEvery)
            {
                repeatTimer = 0.0f;
                PerformFunc(Victim);
            }

            TimeLeft -= dt;
            if (TimeLeft <= 0.0f)
            {
                fxObj.Remove(0);
                SubModule.RemoveEffectFromAgent(Victim, this);
            }
        }
    }
}
