using System;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace RimWorld
{
    public class Meteor : Thing
    {
        public int age;
        public MeteorInfo info;
        private static readonly SoundDef OpenSound = SoundDef.Named("Explosion_Fire");

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<int>(ref this.age, "age", 0, false);
            Scribe_Deep.LookDeep<MeteorInfo>(ref this.info, "info");
        }
        public override void Tick()
        {
            this.age++;
            if (this.age > this.info.openDelay)
            {
                this.PodOpen();
            }
        }
        private void PodOpen()
        {
            foreach (Thing current in this.info.containedThings)
            {
                GenExplosion.DoExplosion(this.Position, this.Map, 1.5f, DamageDefOf.Bomb, null, null, null);
                GenPlace.TryPlaceThing(current, this.Position, this.Map, ThingPlaceMode.Near);
            }
            this.info.containedThings.Clear();
            if (this.info.leaveSlag)
            {
                for (int i = 0; i < 1; i++)
                {
                    Thing thing = ThingMaker.MakeThing(ThingDef.Named("ChunkSlag"), null);
                    GenPlace.TryPlaceThing(thing, base.Position, base.Map, ThingPlaceMode.Near);
                }
            }
            Meteor.OpenSound.PlayOneShot(new TargetInfo(this.Position, this.Map, false));
            GenExplosion.DoExplosion(base.Position, this.Map, 4f, DamageDefOf.Flame, null, null, null);
        }
    }
}
