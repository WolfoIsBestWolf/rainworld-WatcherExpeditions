using BepInEx;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Linq;
using System.Collections.Generic;
using MoreSlugcats;
using Expedition;
using RWCustom;
using Menu.Remix;
using Menu;
using UnityEngine.UIElements;
using HarmonyLib;
using Watcher;
using System.Reflection;
 
namespace WatcherExpeditions
{
    public class ArenaStuff
    {
        public static void Start()
        {
            On.MultiplayerUnlocks.ClassUnlocked += MultiplayerUnlocks_ClassUnlocked;
            //On.PlayerGraphics.ApplyPalette += PlayerGraphics_ApplyPalette;
 
        }
 

        private static void PlayerGraphics_ApplyPalette(On.PlayerGraphics.orig_ApplyPalette orig, PlayerGraphics self, RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
        {
            orig(self, sLeaser, rCam, palette);
            if (rCam.room.game.IsArenaSession)
            {
                //var a = self.GetType().GetField("player").GetValue(self.GetType());
                //Debug.Log(a);
                if (self.CharacterForColor == WatcherEnums.SlugcatStatsName.Watcher)
                {

                    
                    /*switch (this.player.playerState.playerNumber)
                    {
                    }*/
                }
            }
        }

        private static bool MultiplayerUnlocks_ClassUnlocked(On.MultiplayerUnlocks.orig_ClassUnlocked orig, MultiplayerUnlocks self, SlugcatStats.Name classID)
        {
            if (ModManager.Watcher && classID == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return true;
            }
            return orig(self, classID);
        }
    }
}