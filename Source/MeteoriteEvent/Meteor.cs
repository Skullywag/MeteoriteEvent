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
                GenExplosion.DoExplosion(base.Position, 1f, DamageDefOf.Bomb, null, null, null);
                GenExplosion.DoExplosion(base.Position, 1.5f, DamageDefOf.Bomb, null, null, null);
                GenExplosion.DoExplosion(base.Position, 2f, DamageDefOf.Bomb, null, null, null);
                GenExplosion.DoExplosion(base.Position, 4f, DamageDefOf.Flame, null, null, null);
                GenPlace.TryPlaceThing(current, base.Position, ThingPlaceMode.Near);
            }
            this.info.containedThings.Clear();
            if (this.info.leaveSlag)
            {
                for (int i = 0; i < 1; i++)
                {
                    Thing thing = ThingMaker.MakeThing(ThingDef.Named("ChunkSlag"), null);
                    GenPlace.TryPlaceThing(thing, base.Position, ThingPlaceMode.Near);
                }
            }
            SoundStarter.PlayOneShot(Meteor.OpenSound, base.Position);
            GenExplosion.DoExplosion(base.Position, 4f, DamageDefOf.Flame, null, null, null);
        }
    }
}
