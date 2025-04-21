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

namespace WatcherExpeditions
{
    public class NewPerks
    {

        public static void Start()
        {
            On.Menu.UnlockDialog.TogglePerk += BlockPerks;

            return;
            CustomPerks.Register(new CustomPerk[] 
            {
                new Perk_Boomerang(),
                new Perk_PermamentWarps()
            });
            
            //IL.Room.Loaded += AddBoomerang; 
            On.Room.TrySpawnWarpPoint += Room_TrySpawnWarpPoint;

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
            else
            {
                if (message == "unl-watcher_permwarp")
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
                if (ExpeditionGame.activeUnlocks.Contains("unl-watcher_permwarp"))
                {
                    (po.data as WarpPoint.WarpPointData).cycleExpiry = 0;
                }             
            }
            return orig(self, po, saveInRegionState, skipIfInRegionState, deathPersistent);

        }


    }
}