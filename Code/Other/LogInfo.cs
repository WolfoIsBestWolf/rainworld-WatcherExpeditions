using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using RWCustom;
using Menu;
using System;
using Expedition;
using MonoMod.Cil;
using System.IO;
using Mono.Cecil.Cil;
using UnityEngine;
using System.Reflection;
using System.Runtime.Serialization;
using System.Linq;

namespace WatcherExpeditions
{
    public class WLog
    {
        public static void Start()
        {
            On.AbstractSpaceVisualizer.Visibility += AbstractSpaceVisualizer_Visibility;
            if (!WConfig.cfgDebugInfo.Value)
            {
                return;
            }
            On.World.SpawnGhost += DumpOnce;
            On.World.SpawnGhost += DumpMultiple;
            On.Expedition.ChallengeTools.ParseCreatureSpawns += ChallengeTools_ParseCreatureSpawns;       
            //On.Expedition.ChallengeOrganizer.RandomChallenge += ChallengeOrganizer_RandomChallenge;

            //On.MiscWorldSaveData.ctor += TryToSaveLikeWarpPointMap;

           
            IL.RainWorld.BuildTokenCache += WLog.RainWorld_BuildTokenCache;

            On.WinState.CycleCompleted += WinState_CycleCompleted;
            On.Watcher.SpinningTop.NextMinMaxRippleLevel += SpinningTop_NextMinMaxRippleLevel;
            On.RWCustom.Custom.Log += Custom_Log;
         
            //Echo map shows up if you visited Shattered Terrance only
            //On.HUD.Map.MapData.InitWarpData += MapData_InitWarpData;
        }

        public static void AbstractSpaceVisualizer_Visibility(On.AbstractSpaceVisualizer.orig_Visibility orig, AbstractSpaceVisualizer self, bool visibility)
        {
            orig(self, visibility);
            if (WConfig.cfgDebugInfo.Value)
            {
                for (int i = 0; i < self.communityLabels.GetLength(0); i++)
                {
                    for (int j = 0; j < self.communityLabels.GetLength(1); j++)
                    {
                        self.communityLabels[i, j].isVisible = false;
                    }
                }
            }
            
        }

        private static void MapData_InitWarpData(On.HUD.Map.MapData.orig_InitWarpData orig, HUD.Map.MapData self, SaveState saveState)
        {
            orig(self, saveState);
            Debug.Log("MapData_InitWarpData");
            Debug.Log("discoveredWarpRegions");
            Debug.Log("");
            for (int i = 0; i < self.discoveredWarpRegions.Count; i++)
            {
                Debug.Log(self.discoveredWarpRegions[i]);
            }
            Debug.Log("");
            Debug.Log("infectedWarpRegions");
            Debug.Log("");
            for (int i = 0; i < self.infectedWarpRegions.Count; i++)
            {
                Debug.Log(self.infectedWarpRegions[i]);
            }
            Debug.Log("");
            Debug.Log("unencounteredSpinningTopRegions");
            Debug.Log("");
            for (int i = 0; i < self.unencounteredSpinningTopRegions.Count; i++)
            {
                Debug.Log(self.unencounteredSpinningTopRegions[i]);
            }
        }

        private static void Custom_Log(On.RWCustom.Custom.orig_Log orig, string[] values)
        {
            UnityEngine.Debug.Log(string.Join(" ", values));
        }

        private static UnityEngine.Vector2 SpinningTop_NextMinMaxRippleLevel(On.Watcher.SpinningTop.orig_NextMinMaxRippleLevel orig, Room room)
        {
            var temp = orig(room);
            Debug.Log("Min Ripple : " + temp.x);
            Debug.Log("Max Ripple : " + temp.y);
            return temp;

        }

        private static void WinState_CycleCompleted(On.WinState.orig_CycleCompleted orig, WinState self, RainWorldGame game)
        {
            orig(self, game);


            Debug.Log("Ripple : " + (game.session as StoryGameSession).saveState.deathPersistentSaveData.rippleLevel);
            Debug.Log("Min Ripple : " + (game.session as StoryGameSession).saveState.deathPersistentSaveData.minimumRippleLevel);
            Debug.Log("Max Ripple : " + (game.session as StoryGameSession).saveState.deathPersistentSaveData.maximumRippleLevel);
        }

        private static void DumpMultiple(On.World.orig_SpawnGhost orig, World self)
        {
            orig(self);


           

            foreach (var key in Custom.rainWorld.progression.currentSaveState.deathPersistentSaveData.spawnedWarpPoints.Keys)
            {
                Debug.Log("spawnedWarpPoints " + key + " : " + Custom.rainWorld.progression.currentSaveState.deathPersistentSaveData.spawnedWarpPoints[key]);
            }
            foreach (var key in Custom.rainWorld.progression.currentSaveState.deathPersistentSaveData.newlyDiscoveredWarpPoints.Keys)
            {
                Debug.Log("newlyDiscoveredWarpPoints " + key + " : " + Custom.rainWorld.progression.currentSaveState.deathPersistentSaveData.newlyDiscoveredWarpPoints[key]);
            }
            foreach (string key in Custom.rainWorld.progression.currentSaveState.miscWorldSaveData.discoveredWarpPoints.Keys)
            {
                Debug.Log("discoveredWarpPoints " + key + " : " + Custom.rainWorld.progression.currentSaveState.miscWorldSaveData.discoveredWarpPoints[key]);
            }
            
        }

        public static void RainWorld_BuildTokenCache(ILContext il)
        {
            ILCursor c = new(il);
            c.TryGotoNext(MoveType.Before,
             x => x.MatchLdsfld("PlacedObject/Type", "DataPearl"));
            
            c.Index--;  
            c.EmitDelegate<Func<PlacedObject, PlacedObject>> ((placedObject) =>
            {
                //ELog.Log(placedObject.type);
                string fileName3 ="";
                if (placedObject.type == DLCSharedEnums.PlacedObjectType.Stowaway)
                {
                    Debug.Log("Stowaway : " + fileName3);
                }
                else if (placedObject.type == DLCSharedEnums.PlacedObjectType.BigJellyFish)
                {
                    Debug.Log("BigJellyFish : " + fileName3);
                }
                else if (placedObject.type == WatcherEnums.PlacedObjectType.PlacedFireSprite)
                {
                    Debug.Log("PlacedFireSprite : " + fileName3);
                }
                else if (placedObject.type == WatcherEnums.PlacedObjectType.PlacedFrog)
                {
                    Debug.Log("PlacedFrog : " + fileName3);
                }
                return placedObject;
            });

            
        }

        private static void TryToSaveLikeWarpPointMap(On.MiscWorldSaveData.orig_ctor orig, MiscWorldSaveData self, SlugcatStats.Name saveStateNumber)
        {
            bool WatcherThingy = Custom.rainWorld.ExpeditionMode && WConfig.cfgWatcher_WarpMap.Value;
            if (WatcherThingy)
            {
 
            }
            orig(self, saveStateNumber);
        }

        private static void DumpOnce(On.World.orig_SpawnGhost orig, World self)
        {
            orig(self);
            On.World.SpawnGhost -= DumpOnce;
            foreach (var key in Custom.rainWorld.regionSpinningTopRooms.Keys)
            {
                foreach (var list in Custom.rainWorld.regionSpinningTopRooms[key])
                {
                    Debug.Log(key + " :ST: " + list);
                }        
            }

            foreach (var key in Custom.rainWorld.regionWeaverRooms.Keys)
            {
                foreach (var list in Custom.rainWorld.regionWeaverRooms[key])
                {
                    Debug.Log(key + " :VW: " + list);
                }
            }

            foreach (var key in Custom.rainWorld.levelBadWarpTargets)
            {
                Debug.Log("LevelBadWarpTargets " + key);
            }

            foreach (var key in Custom.rainWorld.levelDynamicWarpTargets.Keys)
            {
                foreach (string list in Custom.rainWorld.levelDynamicWarpTargets[key])
                {
                    Debug.Log("LevelDynamicWarpTargets " + key + " : " + list);
                }
            }

            foreach (var key in Custom.rainWorld.regionDynamicWarpTargets.Keys)
            {
                foreach (string list in Custom.rainWorld.regionDynamicWarpTargets[key])
                {
                    Debug.Log("RegionDynamicWarpTargets " + key + " : " + list);
                }
            }

            var ORA = WarpPoint.GetAvailableOuterRimWarpTargets(self.abstractRooms[0], false);
            foreach (var key in ORA)
            {
                Debug.Log("OuterRimWarpTargets " + key);
            }

            foreach (Type type in from TheType in Assembly.GetAssembly(typeof(Challenge)).GetTypes()
                                  where TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(Challenge))
                                  select TheType)
            {
                Debug.Log(type.Name);
            }

        }

        private static Challenge ChallengeOrganizer_RandomChallenge(On.Expedition.ChallengeOrganizer.orig_RandomChallenge orig, bool hidden)
        {
            var temp = orig(hidden);
            //UnityEngine.Debug.Log(temp);
            if (temp == null)
            {
                Debug.Log("NULL CHALLNGE!!!");
            }
            return temp;
        }

        private static void ChallengeTools_ParseCreatureSpawns(On.Expedition.ChallengeTools.orig_ParseCreatureSpawns orig)
        {
            orig();
            //On.Expedition.ChallengeTools.ParseCreatureSpawns -= ChallengeTools_ParseCreatureSpawns;
            foreach (var key in ChallengeTools.creatureSpawns.Keys)
            {
                foreach (var entry in ChallengeTools.creatureSpawns[key])
                {
                    Debug.Log(key+ " : " + entry.creature + " |Spawns: " + entry.spawns + "  |Points: " + entry.points);
                }
            }
 
    }

    }
}