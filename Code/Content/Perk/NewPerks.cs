using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Expedition;
using JetBrains.Annotations;
using Menu;
using Modding.Expedition;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MoreSlugcats;
using RWCustom;
using Steamworks;
using UnityEngine;
using Watcher;


namespace WatcherExpeditions
{
    public class NewPerks
    {

        public static void Start()
        {
            On.Menu.UnlockDialog.TogglePerk += BlockPerks;
            On.Menu.UnlockDialog.ToggleBurden += UnlockDialog_ToggleBurden;

            CustomPerks.Register(new CustomPerk[]
            {
                //new Perk_Boomerang(),
                new Perk_Camo(),
                new Perk_PermamentWarps(),
                new Perk_PoisonSpear(),
                new Perk_DialWarp(),
            });
            CustomBurdens.Register(new CustomBurden[]
            {
                new Burden_Rotted()
            });
            
            IL.Room.Loaded += AddPerkItems;
            On.Room.TrySpawnWarpPoint_PlacedObject_bool += Room_TrySpawnWarpPoint;
            On.PlayerProgression.GetOrInitiateSaveState += PlayerProgression_GetOrInitiateSaveState;
            IL.Menu.FastTravelScreen.ctor += FastTravelScreen_ctor;

            IL.Watcher.WarpMap.LoadWarpConnections += WarpMap_LoadWarpConnections;

            {
                var targetProperty = typeof(RegionState.RippleSpawnEggState).GetProperty("WarpEggThreshold", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                var targetGetter = targetProperty?.GetGetMethod(true);

                var hookMethod = NewPerks.RegionState_RippleSpawnEggState_WarpEggThreshold;

                new MonoMod.RuntimeDetour.Hook(targetGetter, hookMethod);
            }
        }

        private static void UnlockDialog_ToggleBurden(On.Menu.UnlockDialog.orig_ToggleBurden orig, UnlockDialog self, string message)
        {
            orig(self, message);
        }

        public static void AddPerkItems(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("unl-lantern")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<string, Room, string>>((unlock, room) =>
                {
                    if (unlock == "unl-watcher-boomerang")
                    {
                        WorldCoordinate pos12 = new WorldCoordinate(room.abstractRoom.index, room.shelterDoor.playerSpawnPos.x, room.shelterDoor.playerSpawnPos.y, 0);
                        AbstractPhysicalObject abstractPhysicalObject13 = new AbstractPhysicalObject(room.world, WatcherEnums.AbstractObjectType.Boomerang, null, pos12, room.game.GetNewID());
                        room.abstractRoom.entities.Add(abstractPhysicalObject13);
                        abstractPhysicalObject13.Realize();
                    }
                    else if (unlock == "unl-watcher-PoisonSpear")
                    {
                        WorldCoordinate pos12 = new WorldCoordinate(room.abstractRoom.index, room.shelterDoor.playerSpawnPos.x, room.shelterDoor.playerSpawnPos.y, 0);
                        AbstractSpear abstractSpear2 = new AbstractSpear(room.world, null, pos12, room.game.GetNewID(), false);
                        abstractSpear2.poison = 27f;
                        abstractSpear2.poisonHue = 0.33f;
                        room.abstractRoom.entities.Add(abstractSpear2);
                        abstractSpear2.Realize();
                    }
                    return unlock;
                });
            }
            else
            {
                UnityEngine.Debug.Log("WatcherExpeditions: Spawn Boomerang");
            }
        }


        public static void BlockPerks(On.Menu.UnlockDialog.orig_TogglePerk orig, UnlockDialog self, string message)
        {
            if (ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                if (message == "unl-glow")
                {
                    self.PlaySound(SoundID.MENU_Error_Ping);
                    return;
                }
            }
            orig(self, message);
        }


        private static WarpPoint Room_TrySpawnWarpPoint(On.Room.orig_TrySpawnWarpPoint_PlacedObject_bool orig, Room self, PlacedObject po, bool saveInRegionState)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (ExpeditionGame.activeUnlocks.Contains("unl-watcher-permwarp"))
                {
                    // can't use 0 as that makes it a nondynamicwarppoint and it then returns null in tryspawnwarppoint which fails the warp, hopefully this will suffice
                    (po.data as WarpPoint.WarpPointData).cycleExpiry = 999;
                }             
            }
            return orig(self, po, saveInRegionState);
        }

        private static SaveState PlayerProgression_GetOrInitiateSaveState(On.PlayerProgression.orig_GetOrInitiateSaveState orig, PlayerProgression self, SlugcatStats.Name saveStateNumber, RainWorldGame game, ProcessManager.MenuSetup setup, bool saveAsDeathOrQuit)
        {
            SaveState saveState = orig(self, saveStateNumber, game, setup, saveAsDeathOrQuit);

            if (Custom.rainWorld.ExpeditionMode && ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                Dictionary<string, string> watcherMapPortals = WE_Core.FillWatcherMapRegions();
                if (saveState.miscWorldSaveData.discoveredWarpPoints.Count == 0)
                {
                    foreach (var kvp in watcherMapPortals)
                    {
                        saveState.miscWorldSaveData.discoveredWarpPoints[kvp.Key] = kvp.Value;
                    }
                    if (ExpeditionGame.activeUnlocks.Contains("unl-watcher-dialwarp"))
                    {
                        saveState.miscWorldSaveData.hasRippleEggWarpAbility = true; 
                        saveState.miscWorldSaveData.rippleEggsCollected = WConfig.cfgWatcher_DialCharged.Value ? WConfig.cfgWatcher_DialAmount.Value : 0;
                        saveState.miscWorldSaveData.rippleEggsToRespawn.Clear();
                    }
                }
            }
            return saveState;
        }

        private static int RegionState_RippleSpawnEggState_WarpEggThreshold(Func<int> orig)
        {
            if (Custom.rainWorld.ExpeditionMode && ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return WConfig.cfgWatcher_DialAmount.Value;
            }
            return orig();
        }

        private static void WarpMap_LoadWarpConnections(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchCallOrCallvirt(typeof(List<string>).GetProperty("Item").GetGetMethod()),
                x => x.MatchCallOrCallvirt(typeof(string).GetMethod(nameof(string.ToLowerInvariant))),
                x => x.MatchCallOrCallvirt(typeof(List<string>).GetMethod(nameof(List<string>.Contains))),
                x => x.MatchStloc(out _)))
            {
                c.Index--;
                c.EmitDelegate<Func<bool, bool>>(containsResult =>
                {
                    if (Custom.rainWorld.ExpeditionMode &&
                        ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return true;
                    }

                    return containsResult;
                });
            }
            else UnityEngine.Debug.Log(nameof(WarpMap_LoadWarpConnections) + " no workie " + il);
        }

        private static void FastTravelScreen_ctor(ILContext il)
        {
            ILCursor c = new(il);

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld(typeof(ModManager), nameof(ModManager.Expedition))))
            {
                c.EmitDelegate<Func<bool, bool>>(expedition =>
                {
                    if (Custom.rainWorld.ExpeditionMode &&
                        ExpeditionData.slugcatPlayer == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return false;
                    }

                    return expedition;
                });
            }
            else
            {
                UnityEngine.Debug.Log(nameof(FastTravelScreen_ctor) + " no workie " + il);
            }
        }
    }
}