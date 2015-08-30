using System;
using Verse;
namespace RimWorld
{
    public class IncidentWorker_BoulderHit : IncidentWorker
    {
        private const float FogClearRadius = 4.5f;
        public override bool TryExecute(IncidentParms parms)
        {
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
            Random random = new Random();
            int num = random.Next(array.Length);
            ThingDef thingDef = ThingDef.Named(array[num]);
            Thing singleContainedThing = ThingMaker.MakeThing(thingDef);
            IntVec3 dropCenter = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c) && !Find.RoofGrid.Roofed(c) && !Find.FogGrid.IsFogged(c), 1000);
            MeteorUtility.MakeMeteorAt(dropCenter, new MeteorInfo
            {
                SingleContainedThing = singleContainedThing,
                openDelay = 1,
                leaveSlag = false
            });
            Find.LetterStack.ReceiveLetter("Meteor Incoming", "A meteoroid has entered the planets gravity well and come crashing down in a fiery explosion! weeee", LetterType.BadNonUrgent, dropCenter, null);
            return true;
        }
    }
}
