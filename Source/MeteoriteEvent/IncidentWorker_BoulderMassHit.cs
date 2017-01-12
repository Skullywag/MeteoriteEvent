using System;
using Verse;
namespace RimWorld
{
    public class IncidentWorker_BoulderMassHit : IncidentWorker
    {
        public override bool TryExecute(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            IntVec3 dropCenter = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c, map) && !map.roofGrid.Roofed(c) && !map.fogGrid.IsFogged(c), map, 1000);
            GenSpawn.Spawn(ThingDef.Named("Thing_MeteorSpawner"), dropCenter, map);
            Find.LetterStack.ReceiveLetter("Meteor Shower Incoming", "A shower of meteoroids have entered the planets gravity well and come crashing down in a fiery explosion! weeee", LetterType.BadUrgent);           
            return true;
        }
    }
}
