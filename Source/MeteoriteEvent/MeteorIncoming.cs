using System;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace RimWorld
{
    public class MeteorIncoming : Thing
    {
        protected const int MinTicksToImpact = 120;
        protected const int MaxTicksToImpact = 200;
        private const int SoundAnticipationTicks = 100;
        public MeteorInfo contents;
        protected int ticksToImpact = 120;
        private bool soundPlayed;
        private static readonly SoundDef LandSound = SoundDef.Named("MortarRound_PreImpact");
        private static readonly Material ShadowMat = MaterialPool.MatFrom("Things/Special/DropPodShadow", ShaderDatabase.Transparent);
        public override Vector3 DrawPos
		{
			get
			{
				Vector3 result = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.FlyingItem);
				float num = (float)(this.ticksToImpact * this.ticksToImpact) * 0.01f;
				result.x -= num * 0.4f;
				result.z += num * 0.6f;
				return result;
			}
		}
        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.ticksToImpact = Rand.RangeInclusive(120, 200);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
            Scribe_Deep.LookDeep<MeteorInfo>(ref this.contents, "contents");
        }
        public override void Tick()
        {
            MoteThrower.ThrowSmoke(DrawPos, 1f);
            MoteThrower.ThrowLightningGlow(DrawPos, 1f);
            this.ticksToImpact--;
            if (this.ticksToImpact <= 0)
            {
                this.PodImpact();
            }
            if (!this.soundPlayed && this.ticksToImpact < 100)
            {
                this.soundPlayed = true;
                SoundStarter.PlayOneShot(MeteorIncoming.LandSound, base.Position);
            }
        }

        public override void DrawAt(Vector3 drawLoc)
        {
            base.DrawAt(drawLoc);
            float num = 2f + (float)this.ticksToImpact / 100f;
            Vector3 s = new Vector3(num, 1f, num);
            Matrix4x4 matrix = default(Matrix4x4);
            drawLoc.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
            matrix.SetTRS(this.TrueCenter(), base.Rotation.AsQuat, s);
            Graphics.DrawMesh(MeshPool.plane10Back, matrix, MeteorIncoming.ShadowMat, 0);
        }

        private void PodImpact()
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 spawnLoc = base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f);
                MoteThrower.ThrowDustPuff(spawnLoc, 1.2f);
            }
            MoteThrower.ThrowLightningGlow(base.Position.ToVector3Shifted(), 2f);
            Meteor meteor = (Meteor)ThingMaker.MakeThing(ThingDef.Named("Meteor"));
            meteor.info = this.contents;
            GenSpawn.Spawn(meteor, base.Position, this.Rotation);
            this.Destroy(DestroyMode.Vanish);
        }
    }
}
