using System;
using Verse;
namespace RimWorld
{
    public class IncidentWorker_BoulderMassHit : IncidentWorker
    {
        public override bool TryExecute(IncidentParms parms)
        {
            IntVec3 dropCenter = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c) && !Find.RoofGrid.Roofed(c) && !Find.FogGrid.IsFogged(c), 1000);
            GenSpawn.Spawn(ThingDef.Named("Thing_MeteorSpawner"), dropCenter);
            Find.LetterStack.ReceiveLetter("Meteor Shower Incoming", "A shower of meteoroids have entered the planets gravity well and come crashing down in a fiery explosion! weeee", LetterType.BadUrgent);           
            return true;
        }
    }
}
