using Watcher;
using MoreSlugcats;
using System.Collections.Generic;
using Expedition;
using MonoMod.Cil;
using JetBrains.Annotations;
using RWCustom;
using UnityEngine;
using System.IO;
using Modding.Expedition;
using Menu;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using ExpeditionExtraConfig;

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
                new Perk_PoisonSpear()
            });
            CustomBurdens.Register(new CustomBurden[]
            {
                new Burden_Rotted()
            });
            
            IL.Room.Loaded += AddPerkItems; 
            On.Room.TrySpawnWarpPoint += Room_TrySpawnWarpPoint;

            
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


        private static WarpPoint Room_TrySpawnWarpPoint(On.Room.orig_TrySpawnWarpPoint orig, Room self, PlacedObject po, bool saveInRegionState, bool skipIfInRegionState, bool deathPersistent)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (ExpeditionGame.activeUnlocks.Contains("unl-watcher-permwarp"))
                {
                    (po.data as WarpPoint.WarpPointData).cycleExpiry = 0;
                }             
            }
            return orig(self, po, saveInRegionState, skipIfInRegionState, deathPersistent);

        }


    }
}