using System;
using System.Collections.Generic;
using Verse;
namespace RimWorld
{
    public class Thing_MeteorSpawner : ThingWithComps
    {
        private const float FogClearRadius = 4.5f;
        private const int numMaxEvents = 20;
        private const int numMinEvents = 10;
        private const int numMaxMeteors = 4;
        private const int numMinMeteors = 1;
        private int numMeteorEventCount;
        private Random random = new Random();
        public override void SpawnSetup()
        {
            this.numMeteorEventCount = this.random.Next(numMinEvents, numMaxEvents);
            base.SpawnSetup();
        }
        public override void TickRare()
        {
            if (this.random.Next(2) == 1)
            {
                this.DoMeteorEvent();
            }
        }
        private void DoMeteorEvent()
        {
            this.numMeteorEventCount--;
            if (this.numMeteorEventCount < 0)
            {
                Find.LetterStack.ReceiveLetter("Meteor Shower Stopped", "No further meteors detected.", LetterType.Good);
                this.Destroy(0);
                return;
            }
            int num = this.random.Next(numMinMeteors, numMaxMeteors);
            IntVec3 dropCenter = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c) && !Find.RoofGrid.Roofed(c) && !Find.FogGrid.IsFogged(c), 1000);
            string[] array = new string[]
			{
				"SandstoneBoulder",
				"LimestoneBoulder",
				"GraniteBoulder",
				"SlateBoulder",
                "MarbleBoulder",
				"MineralBoulder",
				"SilverBoulder",
				"UraniumBoulder"
			};
            ThingDef thingDef = ThingDef.Named(array[this.random.Next(array.Length)]);
            List<Thing> list = new List<Thing>();
            while (list.Count < num)
            {
                Thing item = ThingMaker.MakeThing(thingDef);
                list.Add(item);
            }
            MeteorUtility.DropThingsNear(dropCenter, list, 1, true, false);
        }
    }
}
