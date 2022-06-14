using System;
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
        public Action<Agent>? StopFunc;

        private string sndString;
        private bool sndLoops;
        private SoundEvent? sndEvent;
        private GameEntity fxObj;
        private float repeatEvery;
        
        public EffectData(Agent attacker, Agent victim, string fxName, Action<Agent> func, string soundString = "", bool soundLoops = false,
            float until = 2.5f, float repeat = 0.0f, Action<Agent>? stopFunc = null)
        {
            Attacker = attacker;
            Victim = victim;
            FXName = fxName;
            PerformFunc = func;
            TimeLeft = until;
            repeatEvery = repeat;
            StopFunc = stopFunc;
            sndString = soundString;
            sndLoops = soundLoops;
        }

        private void Init()
        {
            PerformFunc(Victim);

            fxObj = GameEntity.CreateEmpty(Mission.Current.Scene);
            MatrixFrame mf = new MatrixFrame(Mat3.Identity, new Vec3());
            ParticleSystem.CreateParticleSystemAttachedToEntity(FXName, fxObj, ref mf);

            if (sndString != "")
            {
                int sndEventIdFromString = SoundEvent.GetEventIdFromString(sndString);
                sndEvent = SoundEvent.CreateEvent(sndEventIdFromString, Mission.Current.Scene);
                sndEvent.PlayInPosition(Victim.Position);
            }
            else
                sndEvent = null;
        }

        private float repeatTimer = 0.0f;
        public void PerformTick(float dt)
        {
            if (fxObj == null)
                Init();

            if (sndEvent != null)
            {
                sndEvent.SetPosition(Victim.Position);
                if (sndLoops && !sndEvent.IsPlaying())
                    sndEvent.Play();
            }

            this.setFXObjPos(dt);

            repeatTimer += dt;
            if (repeatEvery > 0.0f && repeatTimer >= repeatEvery)
            {
                repeatTimer = 0.0f;
                PerformFunc(Victim);
            }

            TimeLeft -= dt;
            if (TimeLeft <= 0.0f)
            {
                if (StopFunc != null)
                    StopFunc(Victim);
                if (sndEvent != null)
                    sndEvent.Release();
                fxObj.Remove(0);
                SubModule.RemoveEffectFromAgent(Victim, this);
            }
        }

        private bool fxGoingUp = false;
        private float fxElevation = 0.5f;
        private float fxSpeed = 7.5f;
        private float fxRadius = 0.5f;
        private float fxAngle;
        private void setFXObjPos(float dt)
        {
            if (fxGoingUp)
            {
                fxElevation += dt;
                if (fxElevation >= 1.0f)
                    fxGoingUp = false;
            }
            else
            {
                fxElevation -= dt;
                if (fxElevation <= 0.0f)
                    fxGoingUp = true;
            }

            fxAngle += dt * fxSpeed;
            Vec3 mountFixPos = Victim.HasMount ? new Vec3() : new Vec3(0, 0, 1f);
            fxObj.SetLocalPosition(Victim.Position + mountFixPos + new Vec3((float)Math.Cos(fxAngle)*fxRadius, (float)Math.Sin(fxAngle)*fxRadius, fxElevation));
        }
    }
}
