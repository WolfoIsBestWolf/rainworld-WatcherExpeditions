using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using JetBrains.Annotations;
using RWCustom;
using UnityEngine;
using System.IO;
 
namespace WatcherExpeditions
{
    public class Mission_TimeTravel
    {
        public static bool IsCurrentMission
        {
            get
            {
                return ExpeditionData.activeMission == "WEM_TimeTravel";
            }
        }



        public static bool Added = false;
        public static void Start()
        {
 
        }

        public static void OverrideWarpPoints(Room room)
        {
            int random = Random.Range(0, 3);
            List<string> regions;
            if (random == 2)
            {
                room.game.GetStorySession.saveState.currentTimelinePosition = SlugcatStats.Timeline.Saint;
                regions = SlugcatStats.SlugcatStoryRegions(MoreSlugcatsEnums.SlugcatStatsName.Saint);
                regions.Add("MS");
            }
            else if (random == 1) 
            {
                room.game.GetStorySession.saveState.currentTimelinePosition = SlugcatStats.Timeline.Spear;
                regions = SlugcatStats.SlugcatStoryRegions(MoreSlugcatsEnums.SlugcatStatsName.Spear);
                regions.Add("LC");
            }
            else
            {
                room.game.GetStorySession.saveState.currentTimelinePosition = SlugcatStats.Timeline.White;
                regions = SlugcatStats.SlugcatStoryRegions(SlugcatStats.Name.White);
                regions.Add("OE");
            }
            string dest = regions[Random.Range(0, regions.Count)];
            room.game.overWorld.regions = Region.LoadAllRegions(room.game.TimelinePoint, room.game);
            Region region = room.game.overWorld.GetRegion(dest);
     
        }
     }
}