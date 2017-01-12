using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace RimWorld
{
    public static class MeteorUtility
    {
        private static List<List<Thing>> tempList = new List<List<Thing>>();
        public static void MakeMeteorAt(IntVec3 loc, Map map, MeteorInfo info)
        {
            MeteorIncoming meteorIncoming = (MeteorIncoming)ThingMaker.MakeThing(ThingDef.Named("MeteorIncoming"));
            meteorIncoming.contents = info;
            GenSpawn.Spawn(meteorIncoming, loc, map);
        }
        public static void DropThingsNear(IntVec3 dropCenter, Map map, IEnumerable<Thing> things, int openDelay = 110, bool canInstaDropDuringInit = true, bool leaveSlag = false)
        {
            foreach (Thing current in things)
            {
                List<Thing> list = new List<Thing>();
                list.Add(current);
                MeteorUtility.tempList.Add(list);
            }
            MeteorUtility.DropThingGroupsNear(dropCenter, map, MeteorUtility.tempList, openDelay, canInstaDropDuringInit, leaveSlag);
            MeteorUtility.tempList.Clear();
        }
        public static void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110, bool canInstaDropDuringInit = true, bool leaveSlag = false, bool canRoofPunch = true)
        {
            foreach (List<Thing> current in thingsGroups)
            {
                IntVec3 intVec;
                if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out intVec, true, canRoofPunch))
                {
                    Log.Warning(string.Concat(new object[]
					{
						"DropThingsNear failed to find a place to drop ",
						current.FirstOrDefault<Thing>(),
						" near ",
						dropCenter,
						". Dropping on random square instead."
					}));
                    intVec = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Walkable(map), map, 1000);
                }
                for (int i = 0; i < current.Count; i++)
                {
                    current[i].SetForbidden(true, false);
                }
                if (canInstaDropDuringInit && Find.TickManager.TicksGame < 2)
                {
                    foreach (Thing current2 in current)
                    {
                        GenPlace.TryPlaceThing(current2, intVec, map, ThingPlaceMode.Near);
                    }
                }
                else
                {
                    MeteorInfo dropPodInfo = new MeteorInfo();
                    foreach (Thing current3 in current)
                    {
                        dropPodInfo.containedThings.Add(current3);
                    }
                    dropPodInfo.openDelay = openDelay;
                    dropPodInfo.leaveSlag = leaveSlag;
                    MeteorUtility.MakeMeteorAt(intVec, map, dropPodInfo);
                }
            }
        }
    }
}
