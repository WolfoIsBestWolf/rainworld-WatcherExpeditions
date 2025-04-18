using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using System.Linq;
using RWCustom;
using Menu;
using System;
using Expedition;
using MonoMod.Cil;
using UnityEngine;
using System.Text.RegularExpressions;

namespace WatcherExpeditions
{
    public class TransferWarpProgress
    {
        public static void Start()
        {
            On.SaveState.ctor += SaveState_ctor;
        }

        public Dictionary<string, string> inTransDiscoveredWarpPoints;

        private static void SaveState_ctor(On.SaveState.orig_ctor orig, SaveState self, SlugcatStats.Name saveStateNumber, PlayerProgression progression)
        {
            orig(self, saveStateNumber, progression);

            foreach (KeyValuePair<string, string> warpPoint in self.miscWorldSaveData.discoveredWarpPoints)
            {
                string text = WarpPoint.RoomFromIdentifyingString(warpPoint.Key);
                PlacedObject placedObject = new PlacedObject(PlacedObject.Type.WarpPoint, null);
                string[] s = Regex.Split(warpPoint.Value.Trim(), "><");
                (placedObject.data as WarpPoint.WarpPointData).owner.FromString(s);
                if ((placedObject.data as WarpPoint.WarpPointData).RegionString == null)
                {
                    return;
                }
                bool NotDynamic = (placedObject.data as WarpPoint.WarpPointData).nonDynamicWarpPoint;
                bool OneWay = (placedObject.data as WarpPoint.WarpPointData).oneWay;

                //If Not Dynamic
                //& Not in Echo room?
            }
        }
    }
}