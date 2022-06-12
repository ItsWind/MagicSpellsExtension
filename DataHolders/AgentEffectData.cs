using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MagicSpells.DataHolders
{
    public class AgentEffectData
    {
        public Agent Agent;

        public List<EffectData> ActiveEffects = new();

        //private Skeleton agentSkeleton;
        private string currentParticleEffectName = "";
        private GameEntity? fxObj = null;
        //private Dictionary<sbyte, ParticleSystem> parts = new();

        public AgentEffectData(Agent a)
        {
            Agent = a;
            //agentSkeleton = a.AgentVisuals.GetSkeleton();
            
            /*foreach (sbyte boneIndex in Utils.FXBoneIndices)
            {
                Utils.PrintToMessages(boneIndex.ToString());
                MatrixFrame mF = new MatrixFrame(Mat3.Identity, new Vec3());
                ParticleSystem part = ParticleSystem.CreateParticleSystemAttachedToBone(currentParticleEffectName, agentSkeleton, boneIndex, ref mF);
                parts.Add(boneIndex, part);
            }*/
        }

        public void DoTick(float dt)
        {
            this.fxTick(dt);
            this.effectsTick(dt);
        }

        public void AddEffect(EffectData effect)
        {
            Utils.PrintToMessages("adding effect");
            ActiveEffects.Add(effect);
        }

        public void RemoveEffect(EffectData effect)
        {
            Utils.PrintToMessages("removing effect");
            ActiveEffects.Remove(effect);
        }

        private void effectsTick(float dt)
        {
            foreach (EffectData effect in ActiveEffects.ToList())
                effect.PerformTick(dt);
        }
        
        private void setFXObj(string fxName)
        {
            if (fxObj == null)
                fxObj = GameEntity.CreateEmpty(Mission.Current.Scene, true);
            else
                fxObj.RemoveAllParticleSystems();

            MatrixFrame mF = new MatrixFrame(Mat3.Identity, new Vec3());
            ParticleSystem part = ParticleSystem.CreateParticleSystemAttachedToEntity(fxName, fxObj, ref mF);
        }

        private void moveFXObj(float dt)
        {
            float rotSpeed = 5f;
            float radius = 0.2f;
            float elevationOffset = 1.0f;
            float angle = 0.0f;

            angle += rotSpeed * dt;

            var offset = new Vec3((float)Math.Cos(angle)*radius, (float)Math.Sin(angle)*radius, elevationOffset);
            fxObj.SetLocalPosition(Agent.Position + offset);
        }

        private float fxTimer = 1.0f;
        private void fxTick(float dt)
        {
            if (fxObj != null)
                this.moveFXObj(dt);

            fxTimer -= dt;
            if (fxTimer > 0.0f)
                return;

            Utils.PrintToMessages("running fx update");
            string fxNameToSet = "";
            try
            {
                fxNameToSet = ActiveEffects[0].FXName;
            }
            catch (ArgumentOutOfRangeException e) { }
            if (currentParticleEffectName != fxNameToSet)
            {
                this.setFXObj(fxNameToSet);
                currentParticleEffectName = fxNameToSet;
            }
            fxTimer = 1.0f;
        }
    }
}
