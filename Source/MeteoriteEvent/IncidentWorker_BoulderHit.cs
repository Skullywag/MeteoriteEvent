using System;
using Verse;
namespace RimWorld
{
    public class IncidentWorker_BoulderHit : IncidentWorker
    {
        private const float FogClearRadius = 4.5f;
        public override bool TryExecute(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            string[] array = new string[]
			{
				"SandstoneBoulder",
				"LimestoneBoulder",
				"GraniteBoulder",
				"SlateBoulder",
                "MarbleBoulder",
				"MineralBoulder",
				"SilverBoulder",
                "GoldBoulder",
                "UraniumBoulder",
                "JadeBoulder"
			};
            Random random = new Random();
            int num = random.Next(array.Length);
            ThingDef thingDef = ThingDef.Named(array[num]);
            Thing singleContainedThing = ThingMaker.MakeThing(thingDef);
            IntVec3 dropCenter = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c, map) && !map.roofGrid.Roofed(c) && !map.fogGrid.IsFogged(c), map, 1000);
            MeteorUtility.MakeMeteorAt(dropCenter, map, new MeteorInfo
            {
                SingleContainedThing = singleContainedThing,
                openDelay = 1,
                leaveSlag = false
            });
            Find.LetterStack.ReceiveLetter("Meteor Incoming", "A meteoroid has entered the planets gravity well and come crashing down in a fiery explosion! weeee", LetterType.BadNonUrgent, new TargetInfo(dropCenter, map, false), null);
            return true;
        }
    }
}
