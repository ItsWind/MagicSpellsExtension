using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class AgentFXData
    {
        public Agent AffectedAgent;
		public Skeleton AffectedAgentSkeleton;
        public string FXName;
		public float RunForTime;

		private float timer = 0.0f;

		GameEntity fxObj;
		ParticleSystem part;

		public AgentFXData(Agent agent, string fxName, float time)
        {
            AffectedAgent = agent;
			AffectedAgentSkeleton = agent.AgentVisuals.GetSkeleton();
            FXName = fxName;
			RunForTime = time;

			StartFX();
        }

		public void Tick(float dt)
        {
			if (fxObj != null)
            {
				fxObj.SetLocalPosition(AffectedAgent.Position);
            }

			timer += dt;
			if (timer >= RunForTime)
            {
				AgentFXManager.RemoveAgentFX(AffectedAgent);
            }
        }

		public void RemoveFX()
		{
			Utils.PrintToMessages("attempting removal of FX bone part");
			fxObj.Remove(0);
        }

		public void StartFX()
        {
			fxObj = GameEntity.CreateEmpty(Mission.Current.Scene, false);

			MatrixFrame matrixFrame = new MatrixFrame(Mat3.Identity, new Vec3(0.0f, 0.0f, 0.0f, -1.75f));
			part = ParticleSystem.CreateParticleSystemAttachedToEntity(FXName, fxObj, ref matrixFrame);
        }
    }
}
