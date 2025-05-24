using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Watcher;
using MoreSlugcats;
using RWCustom;
using Menu;
using Expedition;
using MonoMod.Cil;
using UnityEngine;
using JollyCoop;


namespace WatcherExpeditions
{
    internal class JollyCoopAdditions
    {
        //public static bool ForceShowCamoHud;

        public static void Start()
        {
            Hook hook = new Hook(typeof(Player).GetProperty("rippleLevel", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), rippleLevel);

            //Idk how Hud ownership works, presumably always gets Hud of player 1 so
            //Even if Camo Meter showed it wouldnt update because its not their camo

            //Also if they have max ripple they can't like open karma gates or smth
            //Hook hook2 = new Hook(typeof(Player).GetProperty("maxRippleLevel", BindingFlags.Instance | BindingFlags.Public).GetGetMethod(), maxRippleLevel);
            //Hook hook5 = new Hook(typeof(RainWorld).GetProperty("RippleColor", BindingFlags.Static | BindingFlags.Public).GetGetMethod(), newColor);

            On.Player.SpawnPersistentRipple += Player_SpawnPersistentRipple;
            On.JollyCoop.JollyMenu.JollyPlayerSelector.JollyPortraitName += FixWatcherPortrait_Jolly;
            //On.JollyCoop.JollyCustom.SlugClassMenu += AllowWatcherJollyExpedition;
            IL.JollyCoop.JollyMenu.JollySlidingMenu.NextClass += AllowWatcher;
            IL.JollyCoop.JollyCustom.SlugClassMenu += AllowWatcher;
        }
        private static string FixWatcherPortrait_Jolly(On.JollyCoop.JollyMenu.JollyPlayerSelector.orig_JollyPortraitName orig, JollyCoop.JollyMenu.JollyPlayerSelector self, SlugcatStats.Name className, int colorIndexFile)
        {
            if (className == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return "multiplayerportrait31";
            }
            return orig(self, className, colorIndexFile);
        }

        private static void AllowWatcher(ILContext il)
        {
            ILCursor c = new(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("ModManager", "Watcher")))
            {
                c.EmitDelegate<Func<bool, bool>>((self) =>
                {
                    if (Custom.rainWorld.ExpeditionMode)
                    {
                        return false;
                    }
                    return self;
                });
            }
            else
            {
                Debug.Log("JollyCustom_SlugClassMenu Hook Failed");
            }
        }
    

        private static void Player_SpawnPersistentRipple(On.Player.orig_SpawnPersistentRipple orig, Player self, float minRadius, float maxRadius, int cycleExpiry)
        {
            if (self.playerState != null)
            {
                if (self.playerState.playerNumber >= 1)
                {
                    return;
                }
                if (self.room.game != null && self.room.game.IsArenaSession)
                {
                    return;
                }     
            }
            orig(self, minRadius, maxRadius, cycleExpiry);
            //Maybe make it so coop players dont clutter the screen 
        }

        public delegate float orig_rippleLevel(Player self);
        //public delegate float orig_maxRippleLevel(Player self);

        //public delegate Color orig_RippleColor();
        /*public static Color newColor(orig_RippleColor orig)
        {
            return Color.yellow;
        }*/

        public static float rippleLevel(orig_rippleLevel orig, Player self)
        {
            
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (ExpeditionData.slugcatPlayer != WatcherEnums.SlugcatStatsName.Watcher)
                {
                    if (self.SlugCatClass == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return 3f;
                    }
                }
            }
            else if (self.room != null && self.room.game.IsArenaSession && self.SlugCatClass == WatcherEnums.SlugcatStatsName.Watcher)
            {
                return 3f;
            }
            return orig(self);
        }
        /*public static float maxRippleLevel(orig_maxRippleLevel orig, Player self)
        {
            if (Custom.rainWorld.ExpeditionMode)
            {
                if (ExpeditionData.slugcatPlayer != WatcherEnums.SlugcatStatsName.Watcher)
                {
                    if (self.SlugCatClass == WatcherEnums.SlugcatStatsName.Watcher)
                    {
                        return 0.5f;
                    }
                }
            }
            return orig(self);
        }*/
    }
}
