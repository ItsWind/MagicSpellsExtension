using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace MagicSpells
{
    public class AgentFXData
    {
		// wtf is wrong with the indentation on github wtf is going on bro
        public Agent AffectedAgent;
		public Skeleton AffectedAgentSkeleton;
        public string FXName;
		public float RunForTime;

		private float timer = 0.0f;

		Dictionary<GameEntity, Vec3> fxObjs = new();

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
			foreach (KeyValuePair<GameEntity, Vec3> kvp in fxObjs)
				kvp.Key.SetLocalPosition(AffectedAgent.Position + kvp.Value);

			timer += dt;
			if (timer >= RunForTime)
				this.RemoveFX();
        }

		public void RemoveFX()
		{
			foreach (KeyValuePair<GameEntity, Vec3> kvp in fxObjs.ToList())
				kvp.Key.Remove(0);
			AgentFXManager.ActiveFXData[AffectedAgent].Remove(this);
        }

		private void createFXObj()
        {
			GameEntity fxObj = GameEntity.CreateEmpty(Mission.Current.Scene, false);
			Vec3 fxObjPos = new Vec3((float)MBRandom.RandomInt(-25, 25) / 100.0f, (float)MBRandom.RandomInt(-25, 25) / 100.0f, (float)MBRandom.RandomInt(20, 150) / 100.0f);

			MatrixFrame matrixFrame = new MatrixFrame(Mat3.Identity, new Vec3());
			ParticleSystem part = ParticleSystem.CreateParticleSystemAttachedToEntity(FXName, fxObj, ref matrixFrame);

			fxObjs[fxObj] = fxObjPos;
		}

		public void StartFX()
        {
			for(int i = 0; i < 10; i++)
				createFXObj();
        }
    }
}
